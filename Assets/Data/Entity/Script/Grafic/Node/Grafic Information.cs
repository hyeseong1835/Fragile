using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraficInformation : Unit
{
    public Grafic grafic;
    public Controller controller => grafic.con;

    public ValueOutputPort<Grafic> graficPort;
    public ValueOutputPort<Controller> controllerPort;

    protected override void Definition()
    {
        graficPort.Define(this, "Grafic", (flow) => { 
            if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>(); 
            return grafic; 
        });
        controllerPort.Define(this, "Controller", (flow) => {
            if (grafic == null) grafic = flow.stack.gameObject.GetComponent<Grafic>();
            return controller;
        });
    }
}