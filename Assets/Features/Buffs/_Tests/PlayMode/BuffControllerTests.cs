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
        SUT.OnTimerTick += OnBuffTImerTick;

        yield return null;

        Assert.AreEqual(timerCounter, 1);
    }

    [UnityTest]
    public IEnumerator ItReportsWhenBuffAddingIsAttempted()
    {
        int callbacks = 0;

        SUT.OnBeforeBuffAdd += opts => callbacks++;

        SUT.AttemptAdd(new () { Buff = new BuffBase("", 1f), Source = SUT.gameObject });

        Assert.AreEqual(callbacks, 1);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ItReportsWhenBuffRemovingIsAttempted()
    {
        int callbacks = 0;

        SUT.OnBeforeBuffRemoved += opt => callbacks++;

        SUT.AttemptRemove(new BuffRemoveOptions(new BuffBase("", 1f)));

        Assert.AreEqual(callbacks, 1);

        yield return null;
    }

    private int timerCounter = 0;

    private void OnBuffTImerTick(float timer)
    {
        timerCounter++;
    }
}
