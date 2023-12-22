using System.Collections;
using UnityEngine;

namespace Infrastructure.Services.CoroutineRunner
{
    public interface ICoroutineRunnerService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}