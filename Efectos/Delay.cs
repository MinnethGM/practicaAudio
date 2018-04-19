using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        private ISampleProvider fuente;

        public int offsetTiempoMS;

        List<float> muestras = new List<float>();

        private int tamañoBuffer;
        private int cantidadMuestrasBorradas = 0;
        private int cantidadMuestrasTranscurridas = 0;

        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            offsetTiempoMS = 600;
            //50ms - 5000ms

            tamañoBuffer = 20 * fuente.WaveFormat.SampleRate;

        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }


        //Offset es el numero de muestras leídas hasta ahorita
        public int Read(float[] buffer, int offset, int count)
        {
            //Calculo de tiempos
            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido =
               (float) muestras.Count / (float)fuente.WaveFormat.SampleRate;
            
            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestrasOffsetTiempo = (int)
                (((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            //Añadir muestras a nuestro buffer
            for (int i = 0; i < read; i++)
            {
                muestras.Add(buffer[i]);
            }
            //Quitar elementos en nuestro buffer si se paso del maximo
            if(muestras.Count > tamañoBuffer)
            {
                //Ya se paso
                int diferencia = muestras.Count - tamañoBuffer;
                muestras.RemoveRange(0, diferencia);
                cantidadMuestrasBorradas += diferencia;
            }

            //Modificar muestras
            if ( tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {
                    
                    buffer[i] +=
                        muestras[(cantidadMuestrasTranscurridas - cantidadMuestrasBorradas)+
                        i-numMuestrasOffsetTiempo];
                }
            }
            cantidadMuestrasTranscurridas += read;

            
            return read;
        }
    }
}
