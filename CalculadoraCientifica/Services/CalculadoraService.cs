using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraCientifica.Services
{
    internal class CalculadoraService
    {
        public double Sumar(double a, double b) => a + b;

        public double Restar(double a, double b) => a - b;

        public double Multiplicar(double a, double b) => a * b;

        public double Dividir(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("No se puede dividir por cero.");
            return a / b;
        }

        public double RaizCuadrada(double n)
        {
            if (n < 0)
                throw new ArgumentException("No se puede calcular la raíz cuadrada de un número negativo.");
            return Math.Sqrt(n);
        }

        public double Potencia(double baseNum, double exponente)
        {
            return Math.Pow(baseNum, exponente);
        }

        public double LogBase10(double n)
        {
            if (n <= 0)
                throw new ArgumentException("No se puede calcular el logaritmo de un número menor o igual a cero.");
            return Math.Log10(n);
        }

        public double LogNatural(double n)
        {
            if (n <= 0)
                throw new ArgumentException("No se puede calcular el logaritmo de un número menor o igual a cero.");
            return Math.Log(n);
        }

        public double Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("No se puede calcular el factorial de un número negativo.");
            if (n > 170)
                throw new ArgumentException("El número es demasiado grande para calcular su factorial.");

            double resultado = 1;
            for (int i = 2; i <= n; i++)
                resultado *= i;

            return resultado;
        }
    }
}
