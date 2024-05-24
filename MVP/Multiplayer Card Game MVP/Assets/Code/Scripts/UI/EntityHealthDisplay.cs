using Entities;
using UnityEngine;

namespace UI
{
    public class EntityHealthDisplay : HealthDisplay
    {
        [SerializeField]
        private BoardEntity _entity;
        
        
        private void Start()
        {
            SetTargetEntity(_entity);
        }
    }
}