using UnityEngine;

public class SimpleFilter : MonoBehaviour
{    
    [SerializeField] private Shader _shader;
    [SerializeField] private Shader[] _shaders;

    [Header("Silhoette setup")]
    [SerializeField] private Color _nearColor;
    [SerializeField] private Color _farColor;

    private Material _mat;

    private bool _useFilter = true;

    private int _i;

    private void Awake()
    {
        _mat = new Material(_shader);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            _useFilter = !_useFilter;

        if (_shaders != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
                ChangeShader(1);
            else if (Input.GetKeyDown(KeyCode.Q))
                ChangeShader(-1);
        }

        if (_shaders[_i].name == "Hidden/SilhoetteFilter")
            SilhoetteSettings();
    }

    private void ChangeShader(int num)
    {
        _i += num;

        if (_i >= _shaders.Length)
            _i = 0;
        else if (_i < 0)
            _i = _shaders.Length - 1;

        _mat = new Material(_shaders[_i]);

        Debug.Log(_shaders[_i].name);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_useFilter == true)
            UseFilter(source, destination);
        else
            Graphics.Blit(source, destination);
    }

    private void UseFilter(RenderTexture source, RenderTexture destination)
    {
        if (_shaders[_i].name != "Hidden/NeonFilter")
        {
        Graphics.Blit(source, destination, _mat);
        }
        else
        {
            RenderTexture neonTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            RenderTexture thresholdTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            RenderTexture blurTex = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

            Graphics.Blit(source, neonTex, _mat, 0);
            Graphics.Blit(neonTex, thresholdTex, _mat, 1);
            Graphics.Blit(thresholdTex, blurTex, _mat, 2);
            _mat.SetTexture("_SrcTex", neonTex);
            Graphics.Blit(blurTex, destination, _mat, 3);

            RenderTexture.ReleaseTemporary(neonTex);
            RenderTexture.ReleaseTemporary(thresholdTex);
            RenderTexture.ReleaseTemporary(blurTex);
        }
    }

    private void SilhoetteSettings()
    {
        _mat.SetColor("_NearColor", _nearColor);
        _mat.SetColor("_FarColor", _farColor);
    }
}
