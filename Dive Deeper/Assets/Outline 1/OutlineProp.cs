using UnityEngine;

namespace Core
{
    public class OutlineProp : MonoBehaviour
    {
        [SerializeField] private float _outlineWidth;
        [SerializeField] private Color _correctOutlineColor = Color.green;
        [SerializeField] private Color _wrongOutlineColor = Color.red;
        private Color _currentColor;
        private Material _outline;

        void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            Material[] materials = renderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
            {
                Material material = materials[i];

                Material outlineMaterial = Instantiate(material);
                outlineMaterial.SetFloat("_OutlineWidth", _outlineWidth);

                materials[i] = outlineMaterial;
            }

            _currentColor = materials[0].GetColor("_OutlineColor");

            renderer.sharedMaterials = materials;
        }


        public void SetBaseColor()
        {
            _outline.SetColor("_OutlineColor", _currentColor);
        }

        public void ChangeColorCorrect()
        {
            _outline.SetColor("_OutlineColor", _correctOutlineColor);
        }

        public void ChangeColorWrong()
        {
            _outline.SetColor("_OutlineColor", _wrongOutlineColor);
        }
    }
}