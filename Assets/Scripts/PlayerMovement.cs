using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private Vector3 _targetPosition;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private bool _isDefending = false;

    public bool _enemyLocked = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique com o mouse
        {
            HandleInput(Input.mousePosition); // Processa a posição do clique
        }

        if (Input.touchCount > 0) // Toque na tela
        {
            Touch touch = Input.GetTouch(0); // Pega o primeiro toque
            if (touch.phase == TouchPhase.Began) // Apenas ao iniciar o toque
            {
                HandleInput(touch.position); // Processa a posição do toque
            }
        }

        // Movimenta o jogador se necessário
        if (_isMoving)
        {
            MovePlayer();
        }
    }

    void HandleInput(Vector2 screenPosition)
    {
        // Converte a posição da tela em um Ray
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        int layerindex = LayerMask.NameToLayer("Player");
        LayerMask excludeLayer = ~(1 << layerindex);

        // Realiza o Raycast
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, excludeLayer))
        {
            Debug.Log(LayerMask.LayerToName(hitInfo.transform.gameObject.layer));

            // Define o alvo como o ponto clicado
            _targetPosition = hitInfo.point;

            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Atk"))
            {
                // Detectou a Layer "Atk"
                _targetPosition = hitInfo.transform.position;
                _enemyLocked = true;    // Travar no inimigo
                _isDefending = false;   // Desativa o modo de defesa
                _playerAnimator.SetBool("Blocking", _isDefending); // Atualiza a animação de defesa
            }
            else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Def"))
            {
                // Detectou a Layer "Def"
                _targetPosition = hitInfo.transform.position;
                _enemyLocked = true;    // Travar no inimigo
                _isAttacking = false;   // Desativa o modo de ataque
                _playerAnimator.SetBool("Kick", _isAttacking); // Atualiza a animação de ataque
            }
            else
            {
                // Caso nenhuma das Layers seja detectada
                if (_isAttacking)
                {
                    _isAttacking = false;
                    _playerAnimator.SetBool("Kick", _isAttacking);
                }
                else if (_isDefending)
                {
                    _isDefending = false;
                    _playerAnimator.SetBool("Blocking", _isDefending);
                }
                _enemyLocked = false;
            }

            _targetPosition.y = 0f;

            _isMoving = true; // Ativa o movimento do jogador
            _playerAnimator.SetBool("Walking", _isMoving);
        }
    }

    void MovePlayer()
    {
        // Calcula a direção para o alvo
        Vector3 direction = (_targetPosition - _playerTransform.position).normalized;

        // Rotaciona o jogador para olhar na direção do movimento
        if (direction != Vector3.zero) // Evita problemas de rotação quando a direção é zero
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
        }

        // Move o jogador na direção do alvo
        _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, _targetPosition, _moveSpeed * Time.deltaTime);

        // Verifica se o jogador chegou ao destino
        if ((Vector3.Distance(_playerTransform.position, _targetPosition) < 0.1f) || (_isAttacking) || (_isDefending))
        {
            _isMoving = false; // Para o movimento
            _playerAnimator.SetBool("Walking", _isMoving);

            if (_isAttacking)
            {
                _playerAnimator.SetBool("Kick", _isAttacking);
            }
            if (_isDefending)
            {
                _playerAnimator.SetBool("Blocking", _isDefending);
                Debug.Log("BLOQUEIO!");
            }
        }
    }

    public void SetAttack(bool atk)
    {
        _isAttacking = atk;
        if (!atk)
        {
            _playerAnimator.SetBool("Kick", _isAttacking);
        }
    }

    public void SetDefense(bool def)
    {
        _isDefending = def;
        if (!def)
        {
            _playerAnimator.SetBool("Blocking", _isDefending);
        }
    }
}
