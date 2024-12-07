using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public Transform player; // �����
    public float visibilityRadius = 5f; // ������ ���������
    public LayerMask obstacleMask; // ���� �����������
    public int textureSize = 256; // ������ �������� Fog of War

    private Texture2D fogTexture;
    private Color[] fogPixels;
    private bool[] revealedPixels; // ��� �������� ���������� � ���, ��� ��� �������
    private float fogAlpha = 0.8f; // ��������� �������������� ������

    private void Start()
    {
        // ������� �������� ��� Fog of War
        fogTexture = new Texture2D(textureSize, textureSize);
        fogPixels = new Color[textureSize * textureSize];
        revealedPixels = new bool[textureSize * textureSize];

        // ���������� ��� ����� ������� �������
        for (int i = 0; i < fogPixels.Length; i++)
        {
            fogPixels[i] = new Color(0, 0, 0, fogAlpha); // ������ ����� � �������������
            revealedPixels[i] = false; // ��� ������� ������
        }

        fogTexture.SetPixels(fogPixels);
        fogTexture.Apply();
    }

    private void Update()
    {
        UpdateFog();
    }

    private void UpdateFog()
    {
        Vector2 playerPosition = WorldToTextureCoords(player.position);

        int radiusInPixels = Mathf.RoundToInt((visibilityRadius / (float)textureSize) * textureSize);

        for (int y = -radiusInPixels; y <= radiusInPixels; y++)
        {
            for (int x = -radiusInPixels; x <= radiusInPixels; x++)
            {
                Vector2 pixelPosition = playerPosition + new Vector2(x, y);

                // ���������, ����� ����� ���������� � �������� ��������
                if (pixelPosition.x >= 0 && pixelPosition.x < textureSize &&
                    pixelPosition.y >= 0 && pixelPosition.y < textureSize)
                {
                    float distance = Vector2.Distance(pixelPosition, playerPosition);

                    if (distance <= radiusInPixels)
                    {
                        int px = Mathf.RoundToInt(pixelPosition.x);
                        int py = Mathf.RoundToInt(pixelPosition.y);
                        int index = py * textureSize + px;

                        // ��������� ��������� �����
                        Vector2 worldPoint = TextureToWorldCoords(new Vector2(px, py));
                        if (IsPointVisible(worldPoint) && !revealedPixels[index])
                        {
                            // ������ ������� �����
                            fogPixels[index].a = Mathf.Max(fogPixels[index].a - Time.deltaTime, 0f);

                            // �������� ������� ��� �������������
                            if (fogPixels[index].a <= 0f)
                            {
                                revealedPixels[index] = true;
                            }
                        }
                    }
                }
            }
        }

        fogTexture.SetPixels(fogPixels);
        fogTexture.Apply();
    }

    private bool IsPointVisible(Vector2 point)
    {
        Vector2 direction = (point - (Vector2)player.position).normalized;
        float distance = Vector2.Distance(player.position, point);

        RaycastHit2D hit = Physics2D.Raycast(player.position, direction, distance, obstacleMask);
        return hit.collider == null; // ����� �����, ���� ��� �����������
    }

    private Vector2 WorldToTextureCoords(Vector2 worldPosition)
    {
        // ����������� ������� ���������� � ���������� ��������
        Vector2 relativePosition = worldPosition - (Vector2)transform.position;
        return new Vector2(
            Mathf.Clamp01(relativePosition.x / transform.localScale.x) * textureSize,
            Mathf.Clamp01(relativePosition.y / transform.localScale.y) * textureSize
        );
    }

    private Vector2 TextureToWorldCoords(Vector2 texturePosition)
    {
        // ����������� ���������� �������� � ������� ����������
        Vector2 normalizedPosition = new Vector2(
            texturePosition.x / textureSize,
            texturePosition.y / textureSize
        );

        return (Vector2)transform.position + normalizedPosition * (Vector2)transform.localScale;
    }

    private void OnGUI()
    {
        // ���������� �������� Fog of War
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fogTexture);
    }
}