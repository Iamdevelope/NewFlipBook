using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PJW.Book.UI
{
    /// <summary>
    /// 角色选择面板
    /// </summary>
    public class CharacterSelectPanel : BasePanel
    {
        private Transform parent;
        private Sprite[] sprites;
        private GameObject characterBtn;
        private GameObject currentObject;
        private GameObject currentEnterButton;
        private Dictionary<string, GameObject> objects;
        private Dictionary<string, GameObject> allCharacterButton;
        private Button enterBtn;
        private CharacterController characterController;
        public override void Init()
        {
            allCharacterButton = new Dictionary<string, GameObject>();
            objects = new Dictionary<string, GameObject>();
            parent = transform.Find("CharacterList");
            enterBtn = transform.Find("EnterBtn").GetComponent<Button>();
            sprites = Resources.LoadAll<Sprite>("Character/");
            characterBtn = Resources.Load<GameObject>("characterBtn");
            for (int i = 0; i < sprites.Length; i++)
            {
                GameObject go = Instantiate(characterBtn, parent);
                go.transform.GetChild(0).GetComponent<Image>().sprite = sprites[i];
                go.name = sprites[i].name;
                if (!allCharacterButton.ContainsKey(go.name))
                    allCharacterButton[go.name] = go;
                go.GetComponent<Button>().onClick.AddListener(() => CharacterButtonEvent(go.name));
            }

            enterBtn.onClick.AddListener(EnterButtonHandle);
        }
        /// <summary>
        /// 确认事件
        /// </summary>
        private void EnterButtonHandle()
        {
            if (currentObject == null) return;
            GameCore.CurrentObject = currentObject;
            //GameCore.CharacterCamera.SetActive(true);
            currentObject.GetComponentInChildren<InterableAnimal>().Enter();
        }
        /// <summary>
        /// 点击动物头像事件
        /// </summary>
        /// <param name="name"></param>
        private void CharacterButtonEvent(string name)
        {
            GameCore.Instance.SoundManager.PlayAudioClip(SoundManager.CLICK_02);
            if (currentObject != null)
                currentObject.SetActive(false);
            if (currentEnterButton != null)
            {
                currentEnterButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UISprites/click1");
            }
            if (allCharacterButton.ContainsKey(name))
            {
                currentEnterButton = allCharacterButton[name];
                currentEnterButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UISprites/clicked");
            }
            if (!objects.ContainsKey(name))
            {
                GameObject temp = Resources.Load<GameObject>("CharacterPrefabs/" + name);
                if (temp == null) return;
                objects[name] = Instantiate(temp);
                //characterController = objects[name].AddComponent<CharacterController>();
                //characterController.center = new Vector3(0, 0.08f, 0);
                //characterController.radius = 0.05f;
                //characterController.height = 0.3f;
                objects[name].tag = "Player";
                objects[name].transform.GetChild(0).gameObject.AddComponent<InterableAnimal>();
                objects[name].name = temp.name;
            }
            currentObject = objects[name];
            currentObject.SetActive(true);

        }

        public override void Reset(Vector3 scale, float t, string msg = "")
        {
            base.Reset(scale, t, msg);
        }
    }
}