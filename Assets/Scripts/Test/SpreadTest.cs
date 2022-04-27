using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpreadTest : MonoBehaviour
{
    public List<Texture> flags;
    public GameObject panel;
    public GameObject flagBackgroundPrefab;
    public GameObject flagPrefab;


    private void Start()
    {
        RandomShit();
    }


    private void RandomShit()
    {
        for (int i = 0; i < flags.Count; i++)
        {
            var temp = flags[i];
            int randomIndex = Random.Range(i, flags.Count);
            flags[i] = flags[randomIndex];
            flags[randomIndex] = temp;
        }



        foreach (var f in flags)
        {
            var flag = Instantiate(flagPrefab);
            var flagBackground = Instantiate(flagBackgroundPrefab);
            flag.GetComponentInChildren<RawImage>().texture = f;
            flag.GetComponentInChildren<RawImage>().SetNativeSize();




            var randomSize = Random.Range(1.5f, 3);

            float randomXosition = Random.Range(0, panel.GetComponent<RectTransform>().rect.width);
            float randomYosition = Random.Range(0, panel.GetComponent<RectTransform>().rect.height);
            float randomHeight = Random.Range(1.5f, 2);


            flagBackground.transform.SetParent(panel.transform);
         

            flag.GetComponentInChildren<RawImage>().rectTransform.sizeDelta /= randomSize;

            var flagSize = flag.GetComponentInChildren<RawImage>().rectTransform.sizeDelta;
            flag.transform.position = flagBackground.transform.position;
            flag.transform.SetParent(flagBackground.transform);
    
            flagBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(flagSize.x + 4f, flagSize.y + 4f);

            flagBackground.transform.position = new Vector2(randomXosition, randomYosition);
            flagBackground.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }
}
