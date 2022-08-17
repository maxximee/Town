using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Threading.Tasks;
using System;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
namespace MoreMountains.HighroadEngine
{
    /// <summary>
    /// Air Car controller class.
    /// Manages inputs from InputManager and driving of the hover car
    /// You can customize the properties for different types of vehicles.
    /// </summary>
    public class AirCarController : BaseController
    {
        [Header("Alt Actions")]
        public GameObject fireball;
        public GameObject swapRay;
        public float speed = 1000f;
        public float reach = 100f;
        public Transform initPosition;
        public bool canFire = false;

        [Information("Engine Power force.\n", InformationAttribute.InformationType.Info, false)]
        /// the engine's power
        public int DefaultEnginePower = 80000;

        [Range(1, 10)]
        /// the penalty applied when going offroad
        public float OffroadFactor = 2f; // penalty when going offroad

        /// Lateral force applied when steering
        public float LateralSteeringForce = 1f;
        /// the maximum speed
        public int DefaultMaxSpeed = 130;

        [Header("Hover management")]
        [Information("Hover height.\n", InformationAttribute.InformationType.Info, false)]
        /// the distance from the ground at which the vehicle hovers
        public float HoverHeight = 1f;

        [Information("Gravity force applied to the vehicle when ground is too far.\n", InformationAttribute.InformationType.Info, false)]
        /// the force that pushes the vehicle towards the ground
        public float HoverGravityForce = 1f;

        [Information("Hover force applied.\n", InformationAttribute.InformationType.Info, false)]
        /// the force that pushes the vehicle in the air
        public float HoverForce = 1f;

        public float OrientationGroundSpeed = 10f;

        protected RaycastHit _groundRaycastHit;

        protected RaycastHit hit;

        protected Vector3 _startPosition;
        protected Quaternion _startRotation;

        public int MaxSpeed;
        public int EnginePower;

        private Ability currentAbility;
        private bool hasAbility = false;

        public static event Action OnAbilityDoneEvent;

        public static event Action<int> AbilityRemaningEvent;
        public UnityAction OnRespawn;

        private Animator anim;


        /// <summary>
        /// Gets or sets the ground game object.
        /// </summary>
        /// <value>The ground game object.</value>
        protected virtual GameObject GroundGameObject { get; set; }


        /// <summary>
        /// Fixed Update : physics controls
        /// </summary>
        public virtual void FixedUpdate()
        {
            // Input management
            if (CurrentGasPedalAmount > 0)
            {
                Accelerate();
            }

            Rotation();

            // Physics
            Hover();

            OrientationToGround();
        }

        bool coolingDown = false;
        float cooldownCounter = 1;
        private void Update()
        {
            if (coolingDown)
            {
                cooldownCounter -= Time.deltaTime;
                if (cooldownCounter <= 0)
                {
                    coolingDown = false;
                    cooldownCounter = 1;
                }
            } 
        }

        /// <summary>
        /// Manages the acceleration of the vehicle
        /// </summary>
        protected virtual void Accelerate()
        {
            if (Speed < MaxSpeed)
            {
                // If we are offroad, we penalize acceleration and max velocity
                if (GroundGameObject != null && GroundGameObject.tag == "OffRoad")
                {
                    if (_rigidbody.velocity.magnitude > (MaxSpeed / OffroadFactor))
                    {
                        // if the car is going fast enough in an offroad ground, we don't do anything
                        return;
                    }

                    // acceleration is divided by the offroad penalty factor
                    CurrentGasPedalAmount /= OffroadFactor;
                }
                _rigidbody.AddForce(CurrentGasPedalAmount * transform.forward * EnginePower * Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Rotation of the vehicle using steering input
        /// </summary>
        protected virtual void Rotation()
        {
            if (CurrentSteeringAmount != 0)
            {
                transform.Rotate(CurrentSteeringAmount * Vector3.up * SteeringSpeed * Time.fixedDeltaTime);

                Vector3 horizontalVector = transform.right;

                // When rotating, we also apply an opposite tangent force to counter slipping 
                _rigidbody.AddForce(horizontalVector * CurrentSteeringAmount * Time.fixedDeltaTime * LateralSteeringForce * Speed);
            }
        }


        private void reduceAmmo()
        {
            fireAmmo--;
            AbilityRemaningEvent?.Invoke(fireAmmo);
            coolingDown = true;
            if (fireAmmo == 0)
            {
                abilityDone();
            }
        }

        /// <summary>
        /// This method triggers the action button
        /// </summary>
        public override void AltAction()
        {
            if (hasAbility && currentAbility != null)
            {
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                switch (currentAbility.name)
                {
                    case "Fireball":
                        if (!coolingDown)
                        {
                            if (Physics.Raycast(transform.position, fwd, out hit, reach)) //Finds the point where you click with the mouse
                            {
                                FireballAbility fire = (FireballAbility)currentAbility;
                                GameObject projectile = Instantiate(fireball, initPosition.position, Quaternion.identity) as GameObject; //Spawns the selected projectile
                                projectile.tag = "Projectile";
                                projectile.name = name + "-Projectile";
                                projectile.transform.LookAt(hit.point); //Sets the projectiles rotation to look at the point clicked
                                projectile.GetComponent<Rigidbody>().AddForce(transform.forward * ((_rigidbody.velocity.magnitude * 15) + fire.fireballSpeed)); //Set the speed of the projectile by applying force to the rigidbody
                            }
                            reduceAmmo();
                        }
                        break;
                    case "Dash":
                        _rigidbody.AddForce(transform.forward * BoostForce, ForceMode.Impulse);
                        IsOnSpeedBoost = true;
                        StartCoroutine(SpeedBoostLast3Sec());
                        break;
                    case "Swap":
                        if (!coolingDown)
                        {
                            if (Physics.Raycast(transform.position, fwd, out hit, reach)) //Finds the point where you click with the mouse
                            {
                                SwapAbility swap = (SwapAbility)currentAbility;
                                GameObject projectile = Instantiate(swapRay, initPosition.position, Quaternion.identity) as GameObject; //Spawns the selected projectile
                                projectile.tag = "SwapMissile";
                                projectile.name = name + "-Projectile";
                                projectile.transform.LookAt(hit.point); //Sets the projectiles rotation to look at the point clicked
                                projectile.GetComponent<Rigidbody>().AddForce(transform.forward *  ((_rigidbody.velocity.magnitude * 15) + swap.swapRaySpeed)); //Set the speed of the projectile by applying force to the rigidbody
                            }
                            reduceAmmo();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        IEnumerator SpeedBoostLast3Sec()
        {
            yield return new WaitForSeconds(3);
            IsOnSpeedBoost = false;
            // TODO handle this differently, now can use once
            abilityDone();
        }
        private void abilityDone()
        {
            currentAbility = null;
            hasAbility = false;
            OnAbilityDoneEvent?.Invoke();
        }

        private int fireAmmo = 10;
        private float channellingCharge = 3f;

        public void setAbility(Ability abi)
        {
            currentAbility = abi;
            hasAbility = true;
            // TODO do stuff like set ammo, timeouts...
            switch (currentAbility.name)
            {
                case "Fireball":
                    FireballAbility fire = (FireballAbility)currentAbility;
                    fireAmmo = fire.amount;
                    // TODO should wait until roulette is done
                    AbilityRemaningEvent?.Invoke(fireAmmo);
                    break;
                case "Swap":
                    SwapAbility swap = (SwapAbility)currentAbility;
                    fireAmmo = swap.amount;
                    // TODO should wait until roulette is done
                    AbilityRemaningEvent?.Invoke(fireAmmo);
                    break;
                case "Dash":
                    DashAbility dash = (DashAbility)currentAbility;
                    channellingCharge = dash.activeTime;
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Projectile" && !collision.gameObject.name.StartsWith(name))
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.AddForce(Vector3.back);
                changeSpeed();
            }

            if (collision.gameObject.tag == "SwapMissile" && !collision.gameObject.name.StartsWith(name))
            {
                string shooter = collision.gameObject.name.Split(new String[] { "-Projectile" }, StringSplitOptions.None)[0];

                Transform shooterTransform = GameObject.Find(shooter).GetComponent<Transform>();
                Transform myTransform = this.GetComponent<Transform>();

                Vector3 shooterPos = shooterTransform.position;
                Quaternion shooterRot = shooterTransform.rotation;

                shooterTransform.SetPositionAndRotation(myTransform.position, myTransform.rotation);
                myTransform.SetPositionAndRotation(shooterPos, shooterRot);
            }

        }

        private int currentMaxSpeed;
        override protected void Start()
        {
            base.Start();
            DragonDataModel dragonStats = Manager.GetSelectedDragonDataModel();
            MaxSpeed = DefaultMaxSpeed + (int)dragonStats.maxSpeed;
            EnginePower = DefaultEnginePower + ((int)dragonStats.acceleration * 100);
            currentMaxSpeed = MaxSpeed;
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            anim = GetComponentInChildren<Animator>();
        }

        private Boolean isAlreadyHit = false;
        private async void changeSpeed()
        {
            if (isAlreadyHit) {
                return;
            }
            isAlreadyHit = true;

            anim.SetTrigger("die");
            MaxSpeed = 0;
            Debug.Log("Hit, max speed set to:" + MaxSpeed);
            await Task.Delay(TimeSpan.FromSeconds(2));
            MaxSpeed = currentMaxSpeed;
            Debug.Log("Resetting original max speed to:" + MaxSpeed);
            isAlreadyHit = false;
            Accelerate();
        }

        /// <summary>
        /// Management of the hover and gravity of the vehicle
        /// </summary>
        protected virtual void Hover()
        {
            // we enforce isgrounded to false before calculations
            IsGrounded = false;

            // Raycast origin is positionned on the center front of the car
            Vector3 rayOrigin = transform.position + (transform.forward * _collider.bounds.size.z / 2);

            // Raycast to the ground layer
            if (Physics.Raycast(
                rayOrigin,
                -Vector3.up,
                out _groundRaycastHit,
                Mathf.Infinity,
                1 << LayerMask.NameToLayer("Ground")))
            {
                // Raycast hit the ground

                // If distance between vehicle and ground is higher than target height, we apply a force from up to
                // bottom (gravity) to push the vehicle down.
                if (_groundRaycastHit.distance > HoverHeight)
                {
                    // Vehicle is too high, We apply gravity force
                    _rigidbody.AddForce(-Vector3.up * HoverGravityForce * Time.fixedDeltaTime, ForceMode.Acceleration);
                }
                else
                {
                    // if the vehicle is low enough, it is considered grounded
                    IsGrounded = true;

                    // we determine the distance between current vehicle height and wanted height
                    float distanceVehicleToHoverPosition = HoverHeight - _groundRaycastHit.distance;

                    float force = distanceVehicleToHoverPosition * HoverForce;

                    if (_groundRaycastHit.transform.gameObject != null)
                    {
                        // We store the ground object. (Will be used for offroad check)
                        GroundGameObject = _groundRaycastHit.transform.gameObject;
                    }

                    // we add the hoverforce to the rigidbody
                    _rigidbody.AddForce(Vector3.up * force * Time.fixedDeltaTime, ForceMode.Acceleration);
                }
            }
        }

        /// <summary>
        /// Manages orientation of the vehicle depending ground surface normale 
        /// </summary>
        protected virtual void OrientationToGround()
        {
            var rotationTarget = Quaternion.FromToRotation(transform.up, _groundRaycastHit.normal) * transform.rotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, Time.fixedDeltaTime * OrientationGroundSpeed);
        }

        public virtual bool IsOffRoad
        {
            get
            {
                return (GroundGameObject != null && GroundGameObject.tag == "OffRoad");
            }
        }

        /// <summary>
        /// Draws controller gizmos
        /// </summary>
        public virtual void OnDrawGizmos()
        {
            var collider = GetComponent<BoxCollider>();

            var hoverposition = transform.position + (transform.forward * collider.size.z / 2);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hoverposition, 0.1f);

            if (IsGrounded)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawLine(hoverposition, _groundRaycastHit.point);
        }

        /// <summary>
		/// Resets the position of the vehicle.
		/// </summary>
		public override void Respawn()
        {
            Vector3 resetPosition;
            Quaternion resetRotation;

            // Getting current reset position 
            if (Score == 0)
            {
                resetPosition = _startPosition;
                resetRotation = _startRotation;
            }
            else
            {
                Transform resetTransform = _currentWaypoint == 0 ? _checkpoints[_checkpoints.Length - 1] : _checkpoints[_currentWaypoint - 1];
                resetPosition = resetTransform.position;
                resetRotation = resetTransform.rotation;
            }

            _rigidbody.velocity = Vector3.zero;
            transform.position = resetPosition;
            transform.rotation = resetRotation;

            OnRespawn();
        }
    }
}

