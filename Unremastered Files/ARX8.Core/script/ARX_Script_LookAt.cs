using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace ARX
{
    /// <summary>
    /// Makes this object look at the target transform.
    /// </summary>
    [ExecuteInEditMode]
    public class ARX_Script_LookAt : MonoBehaviour
    {
        public Text mo_text;
        public Transform target;
        public Vector3 vecOffset;
        public bool mb_isCanvasElement = false;


        private void Update()
        {
            if (target == null)
                return;
            if(mb_isCanvasElement)
            {
                transform.eulerAngles = ToolBox.PointCanvasElementAt(transform.position, target.position);
                return;
            }
            else
            {
                transform.LookAt(target, Vector3.up);
                transform.eulerAngles += vecOffset;
            }
            
        }
        

    }
}
