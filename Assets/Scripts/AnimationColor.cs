using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationColor : MonoBehaviour
{
    private Image backgroundColor;

    void Start()
    {
        backgroundColor = GetComponent<Image>();
        backgroundColor.color = new Color32(255, 0, 0 , 255);

    }

    void Update()
    {
        StartCoroutine(PaseColor());
       
    }

    IEnumerator PaseColor() {
        if (backgroundColor.color.r == 1 && backgroundColor.color.g == 0 && backgroundColor.color.b == 0)
        {
            for (int i = 0; i <= 255; i += 1)
            {
                ChangeColor( red:255 , green: (byte)i, blue: 0);
                yield return new WaitForSeconds(0.001f);
                if (i == 255) {
                    for (int j = 255; j >= 0; j -= 1)
                    {
                        ChangeColor(red: (byte)j, green: 255, blue: 0);
                        yield return new WaitForSeconds(0.001f);
                        if(j == 0) {
                            for (int k = 0; k <= 255; k += 1) {
                                ChangeColor(red: 0, green: 255, blue: (byte)k);
                                yield return new WaitForSeconds(0.001f);
                                if (k == 255) {
                                    for (int l = 255; l >= 0; l -= 1) {
                                        ChangeColor(red: 0, green: (byte)l, blue: 255);
                                        yield return new WaitForSeconds(0.001f);
                                        if (l == 0)
                                        {
                                            for (int m = 0; m <= 255; m += 1) {
                                                ChangeColor(red: (byte)m, green: 0, blue: 255);
                                                yield return new WaitForSeconds(0.001f);
                                                if (m == 255)
                                                {
                                                    for (int n = 255; n >= 0; n -= 1) {
                                                        ChangeColor(red: 255, green: 0, blue: (byte)n);
                                                        yield return new WaitForSeconds(0.001f);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                           
                        }
                    }
                }
            }
        }
   



        yield return null;
    }


    void ChangeColor(byte red , byte green , byte blue ) {
        backgroundColor.GetComponent<Image>().color = new Color32(red, green, blue, 255);
    }





}
