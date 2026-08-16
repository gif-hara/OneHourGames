using UnityEngine;

namespace OneHourGames.SlimeSlash
{
    public sealed class SlashController : MonoBehaviour
    {
        public SlashController Spawn(Vector3 originPosition, Vector3 destination)
        {
            var instance = Instantiate(this);
            instance.transform.localPosition = originPosition;
            instance.transform.right = (destination - originPosition).normalized;
            var scale = instance.transform.localScale;
            scale.x = Vector3.Magnitude(destination - originPosition); ;
            instance.transform.localScale = scale;
            return instance;
        }
    }
}
