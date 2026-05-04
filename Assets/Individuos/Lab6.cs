using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

namespace Lab6_namespace
{
    public class Lab6 : MonoBehaviour
    {
        VisualElement botonCrear;
        VisualElement botonGuardar;

        Toggle toggleModificar;
        VisualElement contenedor_dcha;

        List<Individuo6> lista_individuos;
        Individuo6 selecIndividuo;

        Button btnGuardar;
        string path;

        VisualElement tarjeta1;
        VisualElement tarjeta2;
        VisualElement tarjeta3;
        VisualElement tarjeta4;

        TextField input_nombre;
        TextField input_apellido;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            /*
            plantilla = root.Q("plantilla");
            input_nombre = root.Q<TextField>("InputNombre");
            input_apellido = root.Q<TextField>("InputApellido");

            individuoPrueba = new Individuo("Perico", "Palotes");

            Tarjeta tarjetaPrueba = new Tarjeta(plantilla, individuoPrueba); */

            contenedor_dcha = root.Q<VisualElement>("Dcha");
            toggleModificar = root.Q<Toggle>("ToggleModificar");
            botonCrear = root.Q<Button>("BotonCrear");

            botonCrear.RegisterCallback<ClickEvent>(NuevaTarjeta);


            botonGuardar = root.Q<Button>("BotonGuardar");

            botonGuardar.RegisterCallback<ClickEvent>(GuardaTarjeta);



            tarjeta1 = root.Q("Tarjeta1");
            tarjeta2 = root.Q("Tarjeta2");
            tarjeta3 = root.Q("Tarjeta3");
            tarjeta4 = root.Q("Tarjeta4");

            input_nombre = root.Q<TextField>("InputNombre");
            input_apellido = root.Q<TextField>("InputApellido");

            path = Application.persistentDataPath + "/individuos.json";

            //if (System.IO.File.Exists(path))
            //{
            //    string json = System.IO.File.ReadAllText(path);
            //    lista_individuos = JsonHelperIndividuo.FromJson<Individuo6>(json);

            //    // restaurar imágenes
            //    foreach (var i in lista_individuos)
            //        i.CargarImagen();
            //}
            //else
            //{
                //Debug.Log("AAAAAAAAAAaaa");
                lista_individuos = Basedatos.getData();
            //}
            contenedor_dcha.RegisterCallback<ClickEvent>(seleccionTarjeta);

            input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);

            VisualElement header = root.Q("headerImg");

            header.Query<VisualElement>()
                  .ForEach(img => img.RegisterCallback<ClickEvent>(CambioImagen));

            InitializeUI();

            //btnGuardar = root.Q<Button>("BtnGuardar");
            //btnGuardar.clicked += GuardarDatos;

            //path = Application.persistentDataPath + "/individuos.json";
        }
        /*
        void SeleccionIndividuo(ClickEvent evt)
        {
            string nombre = plantilla.Q<Label>("Nombre").text;
            string apellido = plantilla.Q<Label>("Apellido").text;

            Debug.Log(nombre);

            input_nombre.SetValueWithoutNotify(nombre);
            input_apellido.SetValueWithoutNotify(apellido);
        }
        */
        void GuardaTarjeta(ClickEvent evt)
        {
            string path = Application.persistentDataPath + "/individuos.json";
            

            ListaIndividuos lista = new ListaIndividuos();
            lista.lista = lista_individuos;

            string json = JsonHelperIndividuo.ToJson(lista_individuos, true);

            System.IO.File.WriteAllText(path, json);

            Debug.Log("Guardado en: " + path);
        }

        void NuevaTarjeta(ClickEvent evt)
        {
            if (!toggleModificar.value)
            {
                VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("plantilla");
                VisualElement tarjetaPlantilla = plantilla.Instantiate();

                contenedor_dcha.Add(tarjetaPlantilla);
                string listaToJson = JsonHelperIndividuo.ToJson(lista_individuos, true);
                List<Individuo6> jsonToLista = JsonHelperIndividuo.FromJson<Individuo6>(listaToJson);
                jsonToLista.ForEach(elem =>
                {
                    Debug.Log(elem.Nombre + " " + elem.Apellido);
                });


                Individuo6 individuo = new Individuo6(input_nombre.value, input_apellido.value);
                lista_individuos.Add(individuo);

                Tarjeta6 tarjeta = new Tarjeta6(tarjetaPlantilla, individuo);
                selecIndividuo = individuo;
            }
        }


        void CambioNombre(ChangeEvent<string> evt)
        {
            /*
            Label nombreLabel = plantilla.Q<Label>("Nombre");
            nombreLabel.text = evt.newValue;
            */
            if (toggleModificar.value)
            {
                selecIndividuo.Nombre = evt.newValue;
            }
        }

        void CambioApellido(ChangeEvent<string> evt)
        {
            /*
            Label apellidoLabel = plantilla.Q<Label>("Apellido");
            apellidoLabel.text = evt.newValue;
            */
            if (toggleModificar.value)
            {
                selecIndividuo.Apellido = evt.newValue;
            }
        }

        void CambioImagen(ClickEvent e)
        {
            if (selecIndividuo == null)
                return;

            VisualElement btn = e.currentTarget as VisualElement;

            Texture2D tex = btn.resolvedStyle.backgroundImage.texture;

            // Solo cambiamos si la imagen existe
            if (tex != null)
            {
                selecIndividuo.Imagen = tex;
            }
        }

        void GuardarDatos()
        {
            string json = JsonHelperIndividuo.ToJson(lista_individuos, true);
            System.IO.File.WriteAllText(path, json);

            Debug.Log("Datos guardados en: " + path);
        }

        void seleccionTarjeta(ClickEvent e)
        {
            //  IMPORTANTE: usar target, NO currentTarget
            VisualElement tarjeta = e.target as VisualElement;

            // Subir por la jerarquía hasta encontrar la tarjeta real
            while (tarjeta != null && tarjeta.userData == null)
            {
                tarjeta = tarjeta.parent;
            }

            if (tarjeta == null)
                return;

            selecIndividuo = tarjeta.userData as Individuo6;

            if (selecIndividuo == null)
                return;

            // Actualizar UI
            input_nombre.SetValueWithoutNotify(selecIndividuo.Nombre);
            input_apellido.SetValueWithoutNotify(selecIndividuo.Apellido);
            toggleModificar.value = true;

            tarjetas_borde_negro();
            tarjetas_borde_blanco(tarjeta);

            
        }

        void tarjetas_borde_negro()
        {
            List<VisualElement> lista_tarjetas = contenedor_dcha.Children().ToList();
            lista_tarjetas.ForEach(elem =>
            {
                VisualElement tarjeta = elem.Q("Tarjeta");

                tarjeta.style.borderBottomColor = Color.black;
                tarjeta.style.borderRightColor = Color.black;
                tarjeta.style.borderTopColor = Color.black;
                tarjeta.style.borderLeftColor = Color.black;
            });
        }

        void tarjetas_borde_blanco(VisualElement tar)
        {
            VisualElement tarjeta = tar.Q("Tarjeta");
            tarjeta.style.borderBottomColor = Color.white;
            tarjeta.style.borderRightColor = Color.white;
            tarjeta.style.borderTopColor = Color.white;
            tarjeta.style.borderLeftColor = Color.white;
        }

        void InitializeUI()
        {
            contenedor_dcha.Clear();

            foreach (var ind in lista_individuos)
            {
                VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("plantilla");
                VisualElement tarjetaPlantilla = plantilla.Instantiate();

                contenedor_dcha.Add(tarjetaPlantilla);

                new Tarjeta6(tarjetaPlantilla, ind);
            }
        }
    }

}