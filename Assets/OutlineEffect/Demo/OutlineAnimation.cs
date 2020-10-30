using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

namespace cakeslice
{
    public class OutlineAnimation : MyBehaviour
    {
        bool pingPong = false;
		public float pingPongSpeed = 2f;

		public static OutlineAnimation self;

		bool isActive = false;

		void Awake(){
			self = this;
			outlineEffect = GetComponent<OutlineEffect> ();
		}

		OutlineEffect outlineEffect;
        // Update is called once per frame
        void FixedUpdate()
        {
			if (!isActive)
				return;
			var c = outlineEffect.lineColor0;

            if(pingPong)
            {
				c.a += Time.fixedDeltaTime * pingPongSpeed;

                if(c.a >= 1)
                    pingPong = false;
            }
            else
            {
				c.a -= Time.fixedDeltaTime * pingPongSpeed;

                if(c.a <= 0)
                    pingPong = true;
            }

            c.a = Mathf.Clamp01(c.a);
            GetComponent<OutlineEffect>().lineColor0 = c;
            GetComponent<OutlineEffect>().UpdateMaterialsPublicProperties();
        }

		public void SetActive(bool t){
			isActive = t;
			if (t) {
				outlineEffect.lineColor0.a = 1;
			} else {
				outlineEffect.lineColor0.a = 0;
			}
		}
    }
}