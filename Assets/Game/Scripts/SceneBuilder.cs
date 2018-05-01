using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SceneBuilder : MonoBehaviour
    {
        [SerializeField] Transform environment;
        [SerializeField] SpriteRenderer groundSample;
        [SerializeField] Sprite[] groundSprites;
        [SerializeField] Transform[] sceneBounds;

        public void BuildScene(int sceneSize)
        {
            var visibleSceneSize = sceneSize + 10;
            var groundOffset = -visibleSceneSize / 2f + .5f;
            for (int i = 0; i < visibleSceneSize; i++)
            {
                for (int j = 0; j < visibleSceneSize; j++)
                {
                    Instantiate(
                        groundSample,
                        new Vector2(i + groundOffset, j + groundOffset),
                        Quaternion.identity,
                        environment)
                        .sprite = groundSprites[Random.Range(0, groundSprites.Length)];
                }
            }

            var lineWidth = .1f;
            sceneBounds[0].position = Vector2.up * sceneSize / 2f;
            sceneBounds[0].localScale = new Vector2(sceneSize + lineWidth, lineWidth);
            sceneBounds[1].position = Vector2.down * sceneSize / 2f;
            sceneBounds[1].localScale = new Vector2(sceneSize + lineWidth, lineWidth);
            sceneBounds[2].position = Vector2.left * sceneSize / 2f;
            sceneBounds[2].localScale = new Vector2(lineWidth, sceneSize + lineWidth);
            sceneBounds[3].position = Vector2.right * sceneSize / 2f;
            sceneBounds[3].localScale = new Vector2(lineWidth, sceneSize + lineWidth);
        }
    }
}
