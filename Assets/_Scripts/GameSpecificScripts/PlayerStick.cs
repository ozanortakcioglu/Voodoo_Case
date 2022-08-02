using UnityEngine;
using DG.Tweening;

public class PlayerStick : MonoBehaviour
{
    [Range(0.3f, 10f)] public float Length = 5f;
    [SerializeField] private float scaleUpLerpSpeed;
    [SerializeField] private GameObject stickModel;
    [SerializeField] private GameObject fakeStickModel;
    [SerializeField] private Transform leftSideTransform;
    [SerializeField] private Transform rightSideTransform;

    private float ActiveLength;

    private void Start()
    {
        ActiveLength = Length;
    }

    private void Update()
    {
        setLength();
    }

    private void setLength()
    {
        ActiveLength = Mathf.Lerp(ActiveLength, Length, Time.deltaTime * scaleUpLerpSpeed);
        stickModel.transform.localScale = new Vector3(stickModel.transform.localScale.x, ActiveLength, stickModel.transform.localScale.z);

        leftSideTransform.localPosition = stickModel.transform.localPosition + new Vector3(-Length, 0, 0);
        rightSideTransform.localPosition = stickModel.transform.localPosition + new Vector3(Length, 0, 0);
    }

    public Vector3 GetSidePosition(bool isLeft)
    {
        if (isLeft)
            return leftSideTransform.position;
        else
            return rightSideTransform.position;
    }

    public void AddStick(float addAmount)
    {
        Length += addAmount;
        fakeStickModel.transform.localScale = new Vector3(fakeStickModel.transform.localScale.z, Length - 0.01f, fakeStickModel.transform.localScale.z);
        Taptic.Light();
    }

    public void MeltStick()
    {
        if (Length <= 0)
            return;

        float meltLenght = 0.15f;
        SoundManager.Instance.PlaySound(SoundTrigger.Cut);


        for (int i = 0; i < 2; i++)
        {
            var cuttedPart = Instantiate(stickModel, null);
            if (i % 2 == 0)
                cuttedPart.transform.position = leftSideTransform.position + Vector3.right * meltLenght;
            else
                cuttedPart.transform.position = rightSideTransform.position + Vector3.left * meltLenght;

            cuttedPart.transform.localScale = new Vector3(cuttedPart.transform.localScale.x, meltLenght, cuttedPart.transform.localScale.z);
            cuttedPart.AddComponent<Rigidbody>().AddForce(Vector3.back, ForceMode.Impulse);
            cuttedPart.AddComponent<SelfDestruct>().lifetime = 2;
        }

        Taptic.Light();
        Length -= meltLenght * 2;
        ActiveLength = Length;
        fakeStickModel.transform.localScale = new Vector3(fakeStickModel.transform.localScale.x, 0, fakeStickModel.transform.localScale.z);
        if (Length <= 0)
        {
            Length = 0;
            PlayerController.Instance.Fail(false);
        }
    }

    public void CutStick(float cutterXPos)
    {
        bool isLeft = cutterXPos < transform.position.x ? true : false;

        float _length = 0;

        Vector3 rotation = new Vector3(0, 80, -90);
        if (cutterXPos < transform.position.x)
            rotation.y = 100;

        SoundManager.Instance.PlaySound(SoundTrigger.Cut);
        EffectsManager.Instance.PlayEffect(EffectTrigger.Cut, new Vector3(cutterXPos, transform.position.y, transform.position.z), rotation, Vector3.one * 0.4f, null);
        Taptic.Light();

        if (isLeft)
            _length = Mathf.Abs(cutterXPos - leftSideTransform.position.x) * 0.5f;
        else
            _length = Mathf.Abs(cutterXPos - rightSideTransform.position.x) * 0.5f;

        Length -= _length;
        if (Length < 0.3f)
            Length = 0.3f;


        var cuttedPart = Instantiate(stickModel, null);
        cuttedPart.transform.position = stickModel.transform.position;
        cuttedPart.transform.localScale = new Vector3(cuttedPart.transform.localScale.x, _length, cuttedPart.transform.localScale.z);

        if (isLeft)
        {
            cuttedPart.transform.position = new Vector3((leftSideTransform.position.x + cutterXPos) / 2, cuttedPart.transform.position.y, cuttedPart.transform.position.z);
            stickModel.transform.Translate(new Vector3(_length * 1, 0, 0), Space.World);
            fakeStickModel.transform.Translate(new Vector3(_length * 1, 0, 0), Space.World);
        }
        else
        {
            cuttedPart.transform.position = new Vector3((rightSideTransform.position.x + cutterXPos) / 2, cuttedPart.transform.position.y, cuttedPart.transform.position.z);
            stickModel.transform.Translate(new Vector3(-_length * 1, 0, 0), Space.World);
            fakeStickModel.transform.Translate(new Vector3(-_length * 1, 0, 0), Space.World);
        }

        ActiveLength = Length;
        fakeStickModel.transform.DOLocalMoveX(0, 0.5f).SetDelay(0.4f).SetEase(Ease.OutBack);
        stickModel.transform.DOLocalMoveX(0, 0.5f).SetDelay(0.4f).SetEase(Ease.OutBack);
        fakeStickModel.transform.localScale = new Vector3(fakeStickModel.transform.localScale.x, 0, fakeStickModel.transform.localScale.z);

        cuttedPart.AddComponent<Rigidbody>().AddForce(Vector3.back, ForceMode.Impulse);
        cuttedPart.AddComponent<SelfDestruct>().lifetime = 2;



    }
}
