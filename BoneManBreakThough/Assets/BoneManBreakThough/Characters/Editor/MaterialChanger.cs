using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(  typeof(CharacterControl) )]
public class MaterialChanger : UnityEditor.Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterControl characterControl = (CharacterControl)target;

        if( GUILayout.Button( "Change Material") )
        {
            characterControl.ChangeMaterial();
        }

    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
