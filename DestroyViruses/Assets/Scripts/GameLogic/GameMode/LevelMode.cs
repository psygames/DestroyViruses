using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace DestroyViruses
{
    // 关卡模式
    public class LevelMode : GameMode
    {
        protected override void OnInit()
        {
            Aircraft.Create();
            OnBegin();
        }

        IDisposable virusCreator = null;
        protected override void OnBegin()
        {
            virusCreator = Observable.Interval(TimeSpan.FromSeconds(3)).Do((ticks) =>
            {
                var virus = VirusBase.Create();
                virus.Reset(new Vector2(UIUtil.width * 0.5f, UIUtil.height));
            }).Subscribe();
        }

        protected override void OnEnd()
        {
            virusCreator.Dispose();
        }

        protected override void OnQuit()
        {

        }

        protected override void OnUpdate(float deltaTime)
        {

        }
    }
}