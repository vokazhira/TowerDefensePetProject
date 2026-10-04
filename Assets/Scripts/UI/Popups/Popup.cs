using Lean.Pool;
using TMPro;
using UnityEngine;

namespace UI.Popups
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _lifetime = 0.5f;

        public void Show(string text, Color color)
        {
            _text.text = text;
            _text.color = color;
            _animator.SetTrigger("IsShow");
            LeanPool.Despawn(this.gameObject, _lifetime);
        }
    }
}