using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour {
  public EnemyType enemyType;

  public enum EnemyType {
    HexagonEnemyMove,
    RectangleEnemyMove,
  }

  public float movementDelay = 0.2f;
  public GameObject currentTarget;

  private void Awake() {
    enemyType = EnemyType.HexagonEnemyMove;
  }

  private void Start() {
    if (enemyType == EnemyType.HexagonEnemyMove) {
      currentTarget = RfHolder.Instance.hexagonList.listPoint[0].gameObject;
      StartCoroutine(MoveThroughPoints());
    }
    else if (enemyType == EnemyType.RectangleEnemyMove) {
      StartCoroutine(MovePointTarget());
    }
  }

  IEnumerator MoveThroughPoints() {
    foreach (var point in RfHolder.Instance.hexagonList.listPoint) {
      yield return StartCoroutine(MoveTo(point.position));
    }

    enemyType = EnemyType.RectangleEnemyMove;
    Start();
  }

  IEnumerator MoveTo(Vector3 pos) {
    yield return transform.DOMove(pos, 0.2f).WaitForCompletion();
  }

  IEnumerator MovePointTarget() {
    if (RfHolder.Instance.rectangleList.listPoint.Count == 0) {
      yield break;
    }

    int randomIndex =
      Random.Range(0, RfHolder.Instance.rectangleList.listPoint.Count);
    Vector3 targetPosition =
      RfHolder.Instance.rectangleList.listPoint[randomIndex].position;
    RfHolder.Instance.rectangleList.listPoint.RemoveAt(randomIndex);

    gameObject.transform.DOMove(targetPosition, 0.2f);
    yield return new WaitForSeconds(2.5f);
    StartCoroutine(MoveUpDownInSync());
  }

  IEnumerator MoveUpDownInSync() {
    float moveAmountUp = 0.5f;
    float moveAmountDown = 0.5f; 
    float delayBetweenMovements = 1f;

    while (true) {
      float startTime = Time.time;

      foreach (var enemy in RfHolder.Instance.map.listEnemy) {
        enemy.transform.DOMoveY(enemy.transform.position.y + moveAmountUp,
          0.5f);
      }

      var time = startTime;
      yield return new WaitUntil(() =>
        Time.time >= time + delayBetweenMovements);

      startTime = Time.time;

      foreach (var enemy in RfHolder.Instance.map.listEnemy) {
        enemy.transform.DOMoveY(enemy.transform.position.y - moveAmountDown,
          0.5f);
      }

      yield return new WaitUntil(() =>
        Time.time >= startTime + delayBetweenMovements);
    }
  }
}