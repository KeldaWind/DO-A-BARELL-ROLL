using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileScript : MonoBehaviour, IPoolableObject
{
    [SerializeField] int poolingIndex;
    public void SetPoolingIndex(int index) { poolingIndex = index; }
    public int GetPoolingIndex { get { return poolingIndex; } }

    private void Reset()
    {

#if UNITY_EDITOR
        CommonLayerMaskResource commonLayerMaskResource = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Common Layer Masks Resource.asset", typeof(CommonLayerMaskResource)) as CommonLayerMaskResource;
        if (commonLayerMaskResource != null)
            hitObjectCheckLayerMask = commonLayerMaskResource.projectileLayerMask;
#endif
    }

    [Header("References")]
    [SerializeField] SphereCollider hitbox = default;

    [Header("Collision")]
    [SerializeField] LayerMask hitObjectCheckLayerMask = default;

    Vector3 shootDirection;
    float shootSpeed;
    int projectileDamages;
    DamageTag damageTag;
    float projectileSize;

    TimerSystem lifetimeSystem;

    public virtual void Shoot(ProjectileParameters projectileParameters, Vector3 direction, DamageTag dmgTag)
    {
        shootDirection = direction.normalized;
        shootSpeed = projectileParameters.GetProjectileSpeed.GetValue();
        lifetimeSystem = new TimerSystem(projectileParameters.GetProjectileLifetime.GetValue(), OnProjectileLifetimeEnded);
        lifetimeSystem.StartTimer();
        projectileSize = projectileParameters.GetProjectileSize;
        transform.localScale = Vector3.one * projectileSize;
        projectileDamages = projectileParameters.GetProjectileDamages;

        damageTag = dmgTag;

        TurnRendererToward(shootDirection);

        if (damageTag == DamageTag.Enemy)
            spriteRenderer.color = Color.red;
        else if (damageTag == DamageTag.Player)
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.white;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        lifetimeSystem.UpdateTimer();
        CheckForObjectOnProjectileWay();
        UpdateProjectileMovements();
    }

    public virtual void UpdateProjectileMovements()
    {
        transform.position += shootDirection * shootSpeed * Time.deltaTime;
    }

    public virtual void CheckForObjectOnProjectileWay()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, hitbox.radius * projectileSize, shootDirection, shootSpeed * Time.deltaTime, hitObjectCheckLayerMask);
        foreach(RaycastHit hit in hits)
        {
            DamageableComponent hitReceiver = hit.collider.GetComponent<DamageableComponent>();
            if (hitReceiver != null)
                HitDamageableReceiver(hitReceiver);

            Wall hitWall = hit.collider.GetComponent<Wall>();
            if (hitWall != null)
                HitWall(hitWall);
        }
    }

    public virtual void OnProjectileLifetimeEnded()
    {
        ReturnObjectToPool();
    }

    [Header("References")]
    [SerializeField] SpriteRenderer spriteRenderer = default;

    public void TurnRendererToward(Vector3 lookingDir)
    {
        if (spriteRenderer == null)
            return;

        float rotZ = Mathf.Atan2(lookingDir.y, lookingDir.x) * Mathf.Rad2Deg;
        spriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotZ - 90));
    }

    public void HitDamageableReceiver(DamageableComponent receiver)
    {
        if (receiver.GetDamageTag != damageTag || receiver.GetDamageTag == DamageTag.Environment || damageTag == DamageTag.Environment)
        {
            receiver.Damage(projectileDamages);
            ReturnObjectToPool();
        }
    }

    public void HitWall(Wall wall)
    {
        ReturnObjectToPool();
    }

    #region Pooling
    public bool CheckIfEnteredScreen()
    {
        return true;
    }

    public bool CheckIfScreenKill()
    {
        return false;
    }

    public void SetUpOnPoolInstantiation(int poolIndex)
    {
        gameObject.SetActive(false);
        poolingIndex = poolIndex;
    }

    public void ResetPoolableObject()
    {
        gameObject.SetActive(true);
    }

    public System.Action<ProjectileScript> OnReturnToPool;

    public void ReturnObjectToPool()
    {
        gameObject.SetActive(false);
        if (OnReturnToPool != null)
            OnReturnToPool(this);
        else
            Destroy(gameObject);
    }
    #endregion
}
