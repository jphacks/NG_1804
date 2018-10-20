using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationUpdater : MonoBehaviour {
	public float IntervalSeconds = 1.0f;
	public LocationServiceStatus Status;
	public LocationInfo Location;

	IEnumerator Start()
	{
		while (true)
		{
			this.Status = Input.location.status;
			if (Input.location.isEnabledByUser)
			{
				switch(this.Status)
				{
				case LocationServiceStatus.Stopped:
					Input.location.Start();
					break;
				case LocationServiceStatus.Running:
					this.Location = Input.location.lastData;
					break;
				default:
					break;
				}
			}
			else
			{
				// FIXME 位置情報を有効にして!! 的なダイアログの表示処理を入れると良さそう
				Debug.Log("location is disabled by user");
				Input.location.Start();
			}

			// 指定した秒数後に再度判定を走らせる
			yield return new WaitForSeconds(IntervalSeconds);
		}
	}
}
