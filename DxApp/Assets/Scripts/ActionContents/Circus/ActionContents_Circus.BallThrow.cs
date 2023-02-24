using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Circus : MonoBehaviour
    {
        [Header("BallThrow")]
        [SerializeField] private RectTransform _ball;
        [SerializeField] private RectTransform _brokenScreen;
        private IEnumerator InitBallThrow()
        {
            float ballThrowScale = 0.15f;
            _ball.gameObject.SetActive(true);
            _anim.speed = 1.5f;

            SetBallInitPos(-1f);

            _ball.localScale = Vector3.one;
            _ball.DOJumpAnchorPos(new Vector2(_width * -0.2f, 100f), 500f, 1, 0.75f);
            _ball.transform.DOScale(ballThrowScale, 0.75f).SetEase(Ease.OutQuad);
            SoundManager.Instance.Play("Whoosh_SFX");
            yield return new WaitForSeconds(0.35f);
            _anim.SetTrigger("JumpTurn");
            yield return new WaitForSeconds(0.2f);
            _figureModel.transform.DOJump(_figureModel.transform.position, 1f, 1, 0.4f);
            yield return new WaitForSeconds(0.1f);
            _ball.DOJumpAnchorPos(new Vector2(_width * -1f, 0f), 100f, 1, 0.5f);
            SoundManager.Instance.Play("Slap_SFX");
            yield return new WaitForSeconds(1.5f);

            SetBallInitPos(1f);

            _ball.DOJumpAnchorPos(new Vector2(_width * 0.2f, 100f), 500f, 1, 0.75f);
            _ball.transform.DOScale(ballThrowScale, 0.75f).SetEase(Ease.OutQuad);
            SoundManager.Instance.Play("Whoosh_SFX");
            yield return new WaitForSeconds(0.35f);
            _anim.SetTrigger("JumpTurn");
            yield return new WaitForSeconds(0.2f);
            _figureModel.transform.DOJump(_figureModel.transform.position, 1f, 1, 0.4f);
            yield return new WaitForSeconds(0.1f);
            _ball.DOJumpAnchorPos(new Vector2(_width * 1f, 0f), 100f, 1, 0.5f);
            SoundManager.Instance.Play("Slap_SFX");
            yield return new WaitForSeconds(1.5f);

            SetBallInitPos(0f);

            _ball.DOJumpAnchorPos(new Vector2(0f, 0f), 500f, 1, 0.75f);
            _ball.transform.DOScale(ballThrowScale, 0.75f).SetEase(Ease.OutQuad);
            SoundManager.Instance.Play("Whoosh_SFX");
            yield return new WaitForSeconds(0.35f);
            _anim.SetTrigger("JumpTurn");
            yield return new WaitForSeconds(0.2f);
            _figureModel.transform.DOJump(_figureModel.transform.position, 1f, 1, 0.4f);
            yield return new WaitForSeconds(0.1f);
            SoundManager.Instance.Play("Slap_SFX");
            _ball.DOJumpAnchorPos(new Vector2(0f, 100f), 150f, 1, 0.2f);
            _ball.transform.DOScale(1.4f, 0.2f);
            yield return new WaitForSeconds(0.2f);
            SoundManager.Instance.Play("GlassShatter_SFX");

            _brokenScreen.gameObject.SetActive(true);
            _brokenScreen.DOShakeAnchorPos(0.2f, 300f, 300);
            yield return new WaitForSeconds(1f);

            _ball.DOAnchorPos(new Vector2(0f, _height * -0.04f), 0.75f);
            SoundManager.Instance.Play("Squeak00_SFX");
            yield return new WaitForSeconds(1.5f);
            _ball.DOAnchorPos(new Vector2(0f, _height * -1.15f), 2f).SetEase(Ease.InCubic);
            SoundManager.Instance.Play("Squeak01_SFX");
            yield return new WaitForSeconds(2.25f);

            _anim.speed = 1f;
            _anim.SetTrigger("Startling");
            SoundManager.Instance.Play("FigureSurprised_SFX");
            _customView.ContentsExit(3.5f, 1.5f);
            yield return new WaitForSeconds(4.5f);
            _brokenScreen.gameObject.SetActive(false);
        }

        void SetBallInitPos(float pos)
        {
            _ball.transform.localScale = Vector3.one;
            _ball.anchoredPosition = new Vector3(_width * -0.35f * pos, _height * -0.8f);
        }
    }
}