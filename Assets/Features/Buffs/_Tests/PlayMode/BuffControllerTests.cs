using System.Collections;
using System.Collections.Generic;
using Features.Buffs;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BuffControllerTests
{
    private BuffController SUT;

    [SetUp]
    public void SetUp()
    {
        var source = new GameObject();

        SUT = source.AddComponent<BuffController>();
    }

    [UnityTest]
    public IEnumerator ItReportsWhenTimerTicks()
    {
        SUT.OnTimerTick.AddListener(OnBuffTImerTick);

        yield return null;

        Assert.AreEqual(timerCounter, 1);
    }

    [UnityTest]
    public IEnumerator ItReportsWhenBuffAddingIsAttempted()
    {
        int callbacks = 0;

        SUT.OnBuffAddRequested.AddListener((buff, source) => callbacks++);

        SUT.AttemptAdd(new BuffBase("", 1f), SUT.gameObject);

        Assert.AreEqual(callbacks, 1);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ItReportsWhenBuffRemovingIsAttempted()
    {
        int callbacks = 0;

        SUT.OnBuffRemoveRequested.AddListener((buff) => callbacks++);

        SUT.AttemptRemove(new BuffBase("", 1f));

        Assert.AreEqual(callbacks, 1);

        yield return null;
    }

    private int timerCounter = 0;

    private void OnBuffTImerTick(float timer)
    {
        timerCounter++;
    }
}
