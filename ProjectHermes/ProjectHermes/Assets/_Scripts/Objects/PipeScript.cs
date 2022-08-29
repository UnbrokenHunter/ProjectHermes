using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TarodevController;
using Cinemachine;
using UnityEngine;

namespace ProjectHermes
{
    public class PipeScript : MonoBehaviour
    {

        #region Program Variables


        private string[] options = { "Player Pipe", "Spawner Pipe", "Enemy Pipe", "Useless Pipe" };
       
        // PLAYER
        private BoxCollider2D[] _cols;
        private GameObject _player;

        // SPAWNER
        private List<GameObject> _enemies = new List<GameObject>();
        private float timer;

		#endregion

		#region User Variables 

        [Title("Pipe")]
		[ValueDropdown("PipeType")]
        [SerializeField]
        private string pipeType;

		#region Player

        [ShowIfGroup("@pipeType == options[0]")]
        [Title("Player Settings")]
        [SerializeField] private float _lerpSpeed = 5;

        [ShowIfGroup("@pipeType == options[0]")]
        [SerializeField] private float yOffset;

        [ShowIfGroup("@pipeType == options[0]")]
        [SerializeField] private float _lerpThreshold = .1f;


        [ShowIfGroup("@pipeType == options[0]")]
        [Title("Location")]
        [SerializeField] private GameObject _exitPipe;

        [ShowIfGroup("@pipeType == options[0]")]
        [ValueDropdown("Backgrounds")]
        [Tooltip("What Background should be enabled in this pipe area")]
        [SerializeField] private string _currentBackground;


        [ShowIfGroup("@pipeType == options[0]")]
        [SerializeField] private CinemachineVirtualCamera _currentAreaCamera;

        [ShowIf("@overWorldBackground == null")]
        [SerializeField] private GameObject overWorldBackground;

        [ShowIf("@warpedBackground == null")]
        [SerializeField] private GameObject warpedBackground;


        #endregion

        #region Spawner 

        [ShowIfGroup("@pipeType == options[1]")]
        [Title("Spawner Settings")]
        [SerializeField] private GameObject spawnerEnemy;

        [ShowIfGroup("@pipeType == options[1]")]
        public bool mustSpawn = true;

        [ShowIfGroup("@pipeType == options[1]")]
        [Tooltip("Bigger number = Slower Spawn")]
        [SerializeField] private float spawnRate = 1;

        [ShowIfGroup("@pipeType == options[1]")]
        [SerializeField] private int maxEnemies;

        [ShowIfGroup("@pipeType == options[1]")]
        [SerializeField] private float lerpSpeed = 1;

        [Title("Enemy Settings")]
        [ShowIfGroup("@pipeType == options[1]")]
        [ValueDropdown("Patrol_or_Coordinates")]
        [HideLabel] [SerializeField] private bool isPatrol;

        [ShowIfGroup("@pipeType == options[1]")]
        [HideIf("isPatrol")]
        [SerializeField] private float walkDistance = 5;

        [ShowIfGroup("@pipeType == options[1]")]
        [SerializeField] private float _walkSpeed = 50;


        [ShowIfGroup("@pipeType == options[1]")]
        [Title("Location")]
        [SerializeField] private Transform _spawnLocation;

        [ShowIfGroup("@pipeType == options[1]")]
        [SerializeField] private Transform _pipeExit;


        #endregion

        #endregion

        #region Unity Methods

        private void Awake()
		{   
            // PLAYER PIPE
            if (pipeType == options[0])
			{
                _cols = this.gameObject.GetComponents<BoxCollider2D>();
                _player = GameObject.Find("Player");
			}
        }

		private void FixedUpdate()
		{
            // SPAWNER PIPE
			if (pipeType == options[1])
			{
                HandleEnemySpawns();
			}
		}

		private void OnTriggerStay2D(Collider2D other)
        {
            // PLAYER PIPE
            if(pipeType == options[0])
			{
                DownPipe(other);
			}
		}

		#endregion

		#region Player

		private void DownPipe(Collider2D other)
		{
            if (other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponent<PlayerController>()._goPipe)
                {
                    if ((this.transform.position.x - _lerpThreshold <= other.transform.position.x && other.transform.position.x <= this.transform.position.x + _lerpThreshold) == false) 
                    {
                        other.gameObject.GetComponent<PlayerController>().MovePlayerToPipe(this.transform, _lerpSpeed);
                        
                    }
                    else
					{
                        other.gameObject.GetComponentInChildren<PlayerAnimator>().PlayerEnterPipe();

                        other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                        other.gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
                        _currentAreaCamera.Priority = 9;
                        other.GetComponentInChildren<PlayerAnimator>()._nextPipe = _exitPipe;
					}
                }
            }
        }

        public void UpPipe()
		{
            print("Up Pipe");

            if(_currentBackground == "Overworld")
			{
                overWorldBackground.SetActive(true);
                warpedBackground.SetActive(false);
			}
            else if(_currentBackground == "Warped")
			{
                overWorldBackground.SetActive(false);
                warpedBackground.SetActive(true);
			}

            _currentAreaCamera.Priority = 10;

            // Teleport player to pipe
            _player.transform.position = this.transform.position + new Vector3(0, yOffset, 0);

        }

		#endregion

		#region Spawner

        private void HandleEnemySpawns()
		{
            if (!mustSpawn) return;
            if (_spawnedEnemyCount() > maxEnemies) return;

            timer += Time.deltaTime;

            if (timer >= spawnRate)
            {
                timer = 0;
                SpawnEnemy();
            }
        }

        private int _spawnedEnemyCount()
		{
            foreach (var enemy in _enemies)
			{
                if (enemy == null)
				{
                    _enemies.Remove(enemy);
				}
			}
            return _enemies.Count + 1;
		}

        private void SpawnEnemy()
		{
            GameObject enemy = Instantiate(spawnerEnemy, _spawnLocation.position, spawnerEnemy.transform.rotation, transform.parent);
            _enemies.Add(enemy);

            AIPatrol patrol = enemy.GetComponent<AIPatrol>();
            if (gameObject.transform.rotation.eulerAngles.z == 90) patrol._startLeft = true;

            patrol.mustPatrol = false;
            patrol.isPatrol = isPatrol;
            patrol.walkDistance = walkDistance;
            patrol._walkSpeed = _walkSpeed;

            EnemyPipeScript script = enemy.GetComponent<EnemyPipeScript>();
            script.inPipe = true;
            script.spawn = _spawnLocation;
            script.exit = _pipeExit;
            script.speed = lerpSpeed;

            script.DisableColliders();

		}

		#endregion

		#region Inspector Stuff

		private static IEnumerable PipeType = new ValueDropdownList<string>()
            {
                { "Player Pipe" },
                { "Spawner Pipe" },
                { "Enemy Pipe" },
                { "Useless Pipe" }
            };

        private static IEnumerable Backgrounds = new ValueDropdownList<string>()
            {
                { "Overworld" },
                { "Warped" },
            };

        private static IEnumerable Patrol_or_Coordinates = new ValueDropdownList<bool>()
            {
                { "Patrol", true },
                { "Distance", false },
            };

        #endregion
    }
}
