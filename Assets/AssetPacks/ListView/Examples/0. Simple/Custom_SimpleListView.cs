using UnityEngine;

namespace ListView
{
    public class Custom_SimpleListView : MonoBehaviour
    {
        public GameObject prefab; //May be obsolete, unsure.
        public int dataOffset; //Where in the inventory pool we currently are.

        public float itemHeight = 1;
        private int range = 3; //How many inventory items are displayed at the same time.

        public bool horizontalInventory;
        public bool customPositions;

        public GameObject[] data; //The entire inventory pool, cannot be from prefabs. Should probably be another type, not GameObject, but GameObject is a placeholder. Was "string[]"
        public GUISkin skin;

        public bool EnableInventoryGUI = false;

        public GameObject[] m_InventoryItemSlot;	//The inventory items currently displayed. Was "TextMesh[]"

        void Start()
        {

            range = m_InventoryItemSlot.Length;
            //Set the inventory positions in a vertical line.
            if (!customPositions)
                for (int i = 0; i < range; i++)
                {
                    if (i > data.Length) //avoid going out of data range
                        break;
                    if (!horizontalInventory)
                        m_InventoryItemSlot[i].transform.position = transform.position + Vector3.down * i * itemHeight;
                    else
                    {
                        m_InventoryItemSlot[i].transform.position = transform.position + Vector3.right * i * itemHeight;
                    }
                    m_InventoryItemSlot[i].transform.parent = transform;
                }
            UpdateList();
        }

        void Update()
        {
            if (Input.GetKeyDown("w"))
            {
                Debug.Log("Scroll next clicked");
                ScrollNext();
            }
            else if (Input.GetKeyDown("s"))
            {
                Debug.Log("Scroll prev clicked");
                ScrollPrev();
            }
        }

        void UpdateList()
        {
           // Debug.Log("Updating list");
            //Update the list after the data offset has been changed, so only items needed visible are visible.
            foreach (var o in data)
            {
                o.SetActive(false);
            }
            for (int i = 0; i < range; i++)
            {
                int dataIdx = i + dataOffset;
              //  Debug.Log("dataIdx: " + dataIdx);
                if (dataIdx >= 0 && dataIdx < data.Length)
                {
                    //List items go in here.
                  //  Debug.Log(m_InventoryItemSlot[i] + " is switched for " + data[dataIdx]);
                    data[dataIdx].SetActive(true);
                    data[dataIdx].transform.position = m_InventoryItemSlot[i].transform.position;
                    //data[dataIdx].transform.position += Vector3.forward;
                    //m_InventoryItemSlot[i] = data[dataIdx];
                }
                //else
                //{
                //    m_InventoryItemSlot[i] = null;
                //}
            }
        }

        void OnGUI()
        {
            if (EnableInventoryGUI)
            { 
            GUI.skin = skin;
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            GUILayout.Label("This is an overly simplistic m_List view. Click the buttons below to scroll, or modify Data Offset in the inspector");
            //Make next button
            if (GUILayout.Button("Scroll Next"))
            {
                Debug.Log("Scroll next clicked");
                ScrollNext();
            }
            //Make prev button
            if (GUILayout.Button("Scroll Prev"))
            {
                Debug.Log("Scroll prev clicked");
                ScrollPrev();
            }
            GUILayout.EndArea();
            }
        }

        void ScrollNext()
        {
            dataOffset++;
            UpdateList();
        }

        void ScrollPrev()
        {
            dataOffset--;
            UpdateList();
        }
    }
}