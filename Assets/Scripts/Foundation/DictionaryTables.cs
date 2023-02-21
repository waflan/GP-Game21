using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

#region DictionaryTexture
    [System.Serializable]
    public class TableTexture: Serialize.TableBase<string, Texture,TableTexturePair>
    {
        
    }

    [System.Serializable]
    public class TableTexturePair : Serialize.KeyAndValue<string, Texture>{

        public TableTexturePair (string key, Texture value) : base (key, value) {

        }
    }
#endregion

#region DictionaryColor
    [System.Serializable]
    public class TableColor: Serialize.TableBase<string, Color,TableColorPair>
    {
        
    }

    [System.Serializable]
    public class TableColorPair : Serialize.KeyAndValue<string, Color>{

        public TableColorPair (string key, Color value) : base (key, value) {

        }
    }
#endregion

