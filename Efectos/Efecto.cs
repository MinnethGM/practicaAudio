using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Efecto : ISampleProvider
    {
        private ISampleProvider fuente;

        private float factor;
        public float Factor
        {
            get
            {
                return factor;
            }
            set
            {
                if (value > 1)
                    factor = 1;
                else if (value < 0)
                    factor = 0;
                else
                    factor = value;
            }
        }

        public Efecto(ISampleProvider fuente)
        {
            this.fuente = fuente;
            Factor = 0.5f;
        }

        public Efecto(ISampleProvider fuente, float factor)
        {
            this.fuente = fuente;
            Factor = factor;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            
            var read = fuente.Read(buffer, offset, count);

            for (int i=0; i < read; i++)
            {
                //Efecto
                buffer[offset + i] *= Factor;
            }

            return read;
        }
    }
}
