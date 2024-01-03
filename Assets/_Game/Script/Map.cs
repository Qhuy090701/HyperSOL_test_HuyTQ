using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
  public ListPoint hexagonList;
  public ListPoint rectangleList;
  public GameObject maybay;
  public float timedelay = 0.5f;
  public MapType mapType;
  public int pointCount;
  public List<GameObject> listEnemy = new List<GameObject>();

  public enum MapType {
    Hexagon,
    Rectangle,
  }

  private void Awake() {
    mapType = MapType.Hexagon;
  }

  private void Start() {
    if (mapType == MapType.Hexagon) {
      hexagonList.gameObject.SetActive(true);
      for (int i = 0; i < hexagonList.listPoint.Count; i++) {
        StartCoroutine(CreatePlane(hexagonList.listPoint[0].position,
          timedelay * i));
      }
    }
  }

  IEnumerator CreatePlane(Vector3 pos, float time) {
    yield return new WaitForSeconds(time);
    GameObject enemy =
      ObjectPool.Instance.SpawnFromPool("Enemy", pos, Quaternion.identity);
    listEnemy.Add(enemy);
  }
}