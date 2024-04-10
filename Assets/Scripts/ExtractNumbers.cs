using System;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using static Cinemachine.DocumentationSortingAttribute;
using System.Drawing;
using UnityEngine.UI;

public class ExtractNumbers : MonoBehaviour
{
    // Variables para almacenar los n�meros
    int temperatura = 0;
    int presion = 0;
    int nivel = 0;
    public TextMeshProUGUI respuesta;
    [SerializeField] Dropdown dropdown;
    [SerializeField] Slider sliderNivel;
    [SerializeField] Slider sliderEstado;

    public void cambiar()
    {
        switch(dropdown.value)
        {
            case 0:
                sliderEstado.value = temperatura;
                sliderNivel.value = temperatura + 2;
                break;
            case 1:
                sliderEstado.value = presion;
                sliderNivel.value = presion + 2;
                break;
            case 2:
                sliderEstado.value = nivel;
                sliderNivel.value = nivel + 2;
                break;
        }
    }

    public void Contar()
    {
        // Expresi�n regular para buscar n�meros
        Regex regex = new Regex(@"\d+");

        // Buscar coincidencias en el texto
        MatchCollection matches = regex.Matches(respuesta.text);



        // Iterar sobre las coincidencias e asignar los n�meros a las variables correspondientes
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
    }
}
