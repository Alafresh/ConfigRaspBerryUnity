using System;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using static Cinemachine.DocumentationSortingAttribute;
using System.Drawing;

public class ExtractNumbers : MonoBehaviour
{
    // Variables para almacenar los números
    int temperatura = 0;
    int presion = 0;
    int nivel = 0;
    public TextMeshProUGUI respuesta;

    public void Contar()
    {
        // Expresión regular para buscar números
        Regex regex = new Regex(@"\d+");

        // Buscar coincidencias en el texto
        MatchCollection matches = regex.Matches(respuesta.text);



        // Iterar sobre las coincidencias e asignar los números a las variables correspondientes
        int i = 0;
        foreach (Match match in matches)
        {
            int num = int.Parse(match.Value); // Convertir el valor de la coincidencia a un entero
            switch (i)
            {
                case 0:
                    temperatura = num;
                    break;
                case 1:
                    presion = num;
                    break;
                case 2:
                    nivel = num;
                    break;
            }
            i++;
        }
    }
    private void Update()
    {
        Contar();
        // Imprimir los números
        Debug.Log("Temperatura: " + temperatura);
        Debug.Log("Presion: " + presion);
        Debug.Log("Nivel de agua: " + nivel);
    }
}
