using System.Collections;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class EventTriggerDeathZone : MonoBehaviour
    {
        [Header("Death Zone Settings")]
        public float timeBeforeDeath = 2f;

        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            StartCoroutine(AttemptToDie(player));
        }

        public IEnumerator AttemptToDie(PlayerManager player)
        {
            yield return new WaitForSeconds(timeBeforeDeath);

            CoreSaveGameManager.instance.LoadGame();

            yield return null;
        }
    }
}
