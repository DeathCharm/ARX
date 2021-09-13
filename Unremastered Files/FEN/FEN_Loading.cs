using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARX;

namespace FEN
{
    public static class Loading
    {
        /// <summary>
        /// Returns the file with the given filename in Resources/image as a sprite.
        /// </summary>
        /// <param name="strFilename"></param>
        /// <returns></returns>
        public static Sprite LoadImage(string strFilename)
        {
            return Resources.Load<Sprite>("image\\" +  strFilename);
        }
        public static GameObject LoadBadEndFlowchart(string strFilename)
        {
            GameObject obj = FEN.Loading.Load("flowchart\\badend\\" + strFilename);
            
            if (obj != null)
                return obj;
            else
                return Load("flowchart\\badend\\default");
        }

        public static GameObject LoadMissionIntroFlowchart(string strFilename)
        {
            GameObject obj = FEN.Loading.Load("flowchart\\missionintro\\" + strFilename);

            if (obj != null)
                return obj;
            else
                return Load("flowchart\\missionintro\\default");
        }
        

        /// <summary>
        /// Returns the file with the given filename in Resources/image as a sprite.
        /// </summary>
        /// <param name="strFilename"></param>
        /// <returns></returns>
        public static Sprite LoadSprite(string strFilename)
        {
            return Resources.Load<Sprite>("sprite\\" + strFilename);
        }

        public static Texture2D LoadTexture2D(string strFilename)
        {
            return Resources.Load<Texture2D>("image\\" + strFilename);
        }

        public static Texture LoadTexture(string strFilename)
        {
            return Resources.Load<Texture>("" + strFilename);
        }

        public static ARX_StatBox_Quad LoadStatQuad(string strDeckName)
        {
            ARX_StatBox_Quad deck = Resources.Load<ARX_StatBox_Quad>(strDeckName);
            if (deck == null)
            {
                Debug.LogError("Could not load stat quad box named " + strDeckName);
            }

            return deck;
        }

        public static ARX_StatBox_Quad LoadUnitStats(string strDeckName)
        {
            ARX_StatBox_Quad deck = Resources.Load<ARX_StatBox_Quad>("unitstats/" + strDeckName);
            if (deck == null)
            {
                Debug.LogError("Could not load stat quad box named " + strDeckName);
            }

            return deck;
        }

        /// <summary>
        /// Returns the file with the given filename in Resources/dungeon as a gameobject.
        /// </summary>
        /// <param name="strDeckName"></param>
        /// <returns></returns>
        public static GameObject LoadDungeonOrTownPrefab(string strDeckName)
        {
            GameObject deck = Resources.Load<GameObject>("dungeon\\" + strDeckName);
            if (deck == null)
            {
                Debug.LogError("Could not load dungeon named " + strDeckName);
            }

            return deck;
        }

        public static ARX_SpreadsheetParser LoadTextSpreadsheet(string strDeckName)
        {
            TextAsset textAsset = LoadTextAsset(strDeckName);
            return new ARX_SpreadsheetParser(textAsset);
        }
        
        public static TextAsset LoadTextAsset(string strDeckName)
        {
            TextAsset deck = Resources.Load<TextAsset>("text/" + strDeckName);
            if (deck == null)
            {
                Debug.LogError("Could not text named text/" + strDeckName);
                deck = new TextAsset();
            }

            return deck;
        }


        public static FEN_Encounter LoadEncounter(string strDeckName)
        {
            FEN_Encounter deck = Resources.Load<FEN_Encounter>("encounter/" + strDeckName);
            if (deck == null)
            {
                Debug.LogError("Could not load encounter named " + strDeckName);
            }

            return deck;
        }

        /// <summary>
        /// Returns a prefab gameobject in the folder "Resources/map"
        /// </summary>
        /// <param name="strMapName"></param>
        /// <returns></returns>
        public static GameObject LoadMap(string strMapName)
        {
            GameObject map = GameObject.Instantiate( Resources.Load<GameObject>("map/" + strMapName));
            if (map == null)
            {
                Debug.LogError("Could not load map named " + strMapName);
            }

            return map;
        }



        public static FEN_Unit LoadUnit(string strFilename)
        {
            Debug.LogError("Loading Unit " + strFilename);

            FEN_Unit oReturn = null;

            try
            {
                oReturn = GameObject.Instantiate(Resources.Load<FEN_Unit>("unit/" + strFilename));
            }
            catch
            {
                if (oReturn == null)
                    oReturn = GameObject.Instantiate(Resources.Load<FEN_Unit>("unit/missingunit"));
            }

            oReturn = oReturn.Clone;

            RPGElementCollections.UnitsInPlay[oReturn.UniqueID] = oReturn;

            oReturn.name = strFilename;


            //Just in case, set the base block of the unit
            oReturn.Stat_Block.Base = 999;
            oReturn.Stat_Block.Current = 0;

            //if(Application.isPlaying)
            oReturn.Initialize();

            //Debug.Log("Loaded unit " + strFilename);
            return oReturn;
        }

        public static FEN_AbilityList LoadDeck(string strDeckName, bool bCloned = true)
        {
            FEN_AbilityList deck = Resources.Load<FEN_AbilityList>("decks\\" + strDeckName);
            if (deck == null)
            {
                Debug.LogError("Could not load deck named " + strDeckName);
                return ScriptableObject.CreateInstance<FEN_AbilityList>();
            }

            if (bCloned)
                return deck.Clone;

            return deck;
        }


        public static FEN_AbilityList LoadShop(string strDeckName)
        {
            FEN_AbilityList deck = Resources.Load<FEN_AbilityList>("shop\\" + strDeckName);
            if (deck == null)
            {
                Debug.LogError("Could not load shop named " + strDeckName);
                return null;
            }

            return deck;
        }


        /// <summary>
        /// Returns the ability with the given filename in Resources/ability as a FEN_Ability.
        /// Set IsDictionaryAbility to true if ability is meant for the Index of all abilities.
        /// </summary>
        /// <param name="strFilename"></param>
        /// <returns></returns>
        public static FEN_Ability LoadAbility(CardIDs.CARDID eID, FEN_Unit owner, bool bIsDictionaryAbility = false, bool bIsClone = true)
        {
            if (owner == null)
                owner = FEN.RPGElementCollections._GlobalUnit;

            string strFilename = eID.ToString().ToLower();
            FEN_Ability oLoad;

            oLoad = Resources.Load<FEN_Ability>("ability\\" + strFilename);

            if (oLoad == null)
            {
                Debug.LogError("Could not load ability " + strFilename);
                Application.Quit();
                return null;
            }

            //Debug.LogWarning("Loading card " + oLoad.nameAndID);
            //Debug.Log(oLoad.nameAndID + " load started.");

            FEN_Ability oReturn = oLoad;

            if (bIsClone)
                oReturn = oLoad.Clone;

            oLoad.Destroy();


            if (Application.isPlaying && bIsDictionaryAbility == false)// && owner != null)
            {
                oReturn.SetOwner(owner);
                oReturn.Initialize(eID, bIsDictionaryAbility);
            }

            oReturn.name = oReturn.name.Split(new string[] { "(Clone" }, System.StringSplitOptions.None)[0];

            oReturn.isDictionaryAbility = bIsDictionaryAbility;
            return oReturn;
        }

        public static FEN_Ability LoadAbility(CardIDs.CARDID eID)
        {
            string strFilename = eID.ToString().ToLower();
            FEN_Ability oLoad;

            oLoad = Resources.Load<FEN_Ability>("ability\\" + strFilename);

            return oLoad;
        }

        /// <summary>
        /// Returns the file with the given filename in Resources/eventorder as an Event Order list
        /// </summary>
        /// <param name="strFilename"></param>
        /// <returns></returns>
        public static ARX_EventOrder LoadEventOrder(string strFilename)
        {
            return Resources.Load<ARX_EventOrder>("eventorder\\" + strFilename);
        }

        /// <summary>
        /// Returns the file with the given filename in Resources/bgm as an audio clip
        /// </summary>
        /// <param name="strFilename"></param>
        /// <returns></returns>
        public static AudioClip LoadBGM(string strFilename)
        {
            return Resources.Load<AudioClip>("bgm\\" + strFilename);
        }

        /// <summary>
        /// Returns the file with the given filename in Resources/sound as an audio clip
        /// </summary>
        /// <param name="strFilename"></param>
        /// <returns></returns>
        public static AudioClip LoadSound(string strFilename)
        {
            return Resources.Load<AudioClip>("sound\\" + strFilename);
        }

        /// <summary>
        /// Returns the file with the given filename in Resources/event as an event.
        /// </summary>
        /// <param name="strFilename"></param>
        /// <returns></returns>
        public static ARX_Event LoadEvent(string strFilename)
        {
            return Resources.Load<ARX_Event>("event\\" + strFilename);
        }

        public static GameObject Load(string strFilename)
        {
            GameObject prefab = Resources.Load(strFilename) as GameObject;
            if (prefab == null)
                return null;

            GameObject obj = GameObject.Instantiate(prefab);

            return obj;
        }

        public static GameObject LoadArena(string strFilename)
        {
            return GameObject.Instantiate(Resources.Load("arena\\" + strFilename) as GameObject);
        }

        public static GameObject LoadPrefab(string strFilename)
        {
            string str = "prefab\\" + strFilename;
            return GameObject.Instantiate(Resources.Load(str) as GameObject);
        }
        

        //public static GameObject LoadModel(string strFilename, FEN_Unit unit = null)
        //{
        //    string strEnemyName = strFilename;
        //    string str = "model\\" + strFilename;
        //    GameObject model = null;

        //    try
        //    {
        //        model = GameObject.Instantiate(Resources.Load(str) as GameObject);
        //    }
        //    catch
        //    {
        //        str = "model\\" + "missingmodel";
        //        model = GameObject.Instantiate(Resources.Load(str) as GameObject);
        //    }
        //    Text t = model.GetComponentInChildren<Text>();

        //    if(t != null && unit != null)
        //       t.text = unit.nameAndID;

        //    return model;
        //}

    }
}