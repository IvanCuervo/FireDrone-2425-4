﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DroneController
{
    /*
	 * La clase DroneSimulator simula el movimiento de un dron implementando la interfaz IDroneDriver.
	 * 
	 * Para simular el vuelo se usa la clase FlightSimulator, que implementa el cálculo del desplazamiento.
	 * 
	 * Para simular el aspecto temporal se crea una tarea de invoca de forma periódica a StepSimulation,
	 * que actualiza la posición cada UpdateIntervalMs. Cada vez que se actualiza la posición se reduce la bateria en una unidad
	 * 
	 * Los tests muestran un ejemplo de uso.
	 */

    public class DroneSimulator : IDroneDriver
    {
        const int DEFAULT_UPDATE_INTERVAL_MS = 1000;
        const int DEFAULT_INITIAL_BATTERY = 1000;

        public int UpdateIntervalMs { get; set; }
        public int InitialBattery { get; set; }

        private CancellationTokenSource _tokenSource;
        private Task _task;

        private FlightSimulator _flightSimulator;

        private Object _statusLock = new System.Object();
        private DroneStatus _status;

        public DroneSimulator()
        {
            UpdateIntervalMs = DEFAULT_UPDATE_INTERVAL_MS;
            InitialBattery = DEFAULT_INITIAL_BATTERY;

            _status = new DroneStatus
            {
                Latitude = 0,
                Longitude = 0,
                Altitude = 0,
                Speed = 0,
                Battery = 0,
                State = DroneState.Stopped
            };
        }

        IDroneCallback _updateCallback = null;

        public void SetUpdateCallback(IDroneCallback callback)
        {
            _updateCallback = callback;
        }

        // Número de pasos necesarios para completar la simulación entre las dos posiciones actuales
        public int GetNumSteps()
        {
            return _flightSimulator.GetNumSteps();
        }

        // Retorna true si la simulación debe continuar
        public bool StepSimulation()
        {
            // Actualiza la posición
            bool arrived = _flightSimulator.StepSimulation();

            // Actualiza el estado
            bool continueSimulation = UpdateStatus(arrived);

            // La simulación termina cuando se acaba la bateria y se llega al destino
            return continueSimulation;
        }

        // Actualiza el estado
        private bool UpdateStatus(bool arrived)
        {
            bool continueSimulation = true;

            // Actualización del estado de forma sincronizada (variable compartida)
            lock (_statusLock)
            {
                _status.Latitude = _flightSimulator.GetCurrentLatitude();
                _status.Longitude = _flightSimulator.GetCurrentLongitude();
                _status.Altitude = _flightSimulator.GetCurrentAltitude();
                _status.Speed = _flightSimulator.GetCurrentSpeed();

                // Se reduce la batería en una unidad
                _status.Battery--;

                if (arrived)
                {
                    _status.State = DroneState.Landed;
                    _status.Altitude = 0;
                    _status.Speed = 0;

                    continueSimulation = false;
                }

                if (_status.Battery == 0)
                {
                    _status.Altitude = 0;
                    _status.Speed = 0;

                    continueSimulation = false;
                }
                if (_updateCallback != null)
                {
                    _updateCallback.Update(_status);
                }
            }

            return continueSimulation;
        }

        // Inicializa la simulación
        public void StartSimulation(Waypoint[] waypoints)
        {
            _flightSimulator = new FlightSimulator(waypoints, UpdateIntervalMs);
            _status.Battery = InitialBattery;
            _status.State = DroneState.Flying;
        }

        // Ejecuta una tarea para simular el plan de vuelo entre la lista de coordenadas
        public void StartFlightPlan(Waypoint[] waypoints)
        {
            _tokenSource = new CancellationTokenSource();
            CancellationToken token = _tokenSource.Token;

            _task = Task.Factory.StartNew(() =>
            {
                StartSimulation(waypoints);

                while (StepSimulation())
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    Task.Delay(UpdateIntervalMs).Wait();
                }
            }, _tokenSource.Token);

            _task.ContinueWith((_task) => Log.Debug("Task simulation finished"));
        }

        // Detiene la tarea de simulación
        public void StopFlightPlan()
        {
            _tokenSource.Cancel();
            try
            {
                _task.Wait();
            }
            catch (AggregateException /*e*/)
            {
                // Excepción esperada tras la cancelación
            }
            finally
            {
                _tokenSource.Dispose();
            }
            lock (_statusLock)
            {
                _status.State = DroneState.Stopped;
            }
        }

        // Obtiene el estado actual del dron
        public DroneStatus GetStatus()
        {
            DroneStatus status;
            lock (_statusLock)
            {
                status = _status;
            }
            return status;
        }
    }
}