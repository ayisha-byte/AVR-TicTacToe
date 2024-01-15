/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class ReticleBehaviour : MonoBehaviour{
  private ARPlaneManager m_planemanager;
  private ARRaycastManager m_raycastmanager;
  private ARPlane m_lockedplane;
  private ARPlane m_currentplane;
  private ARRaycast m_arraycast;
  private GameObject m_arcamera;

  public GameObject m_prefab1;
  public GameObject m_prefab2;
  public GameObject m_prefab3;

  private GameObject[] m_go_array;
  private int m_array_ctr;
  private bool m_button1_activated;
  private bool m_button2_activated;
  private bool m_button3_activated;

  // Start is called before the first frame update
  private void Start(){
    GameObject go_1 = GameObject.Find("AR Session Origin");
    m_raycastmanager = go_1.GetComponent<ARRaycastManager>();
    m_planemanager = go_1.GetComponent<ARPlaneManager>();
    m_go_array = new GameObject[500];
    m_array_ctr = 0;
    m_arcamera = GameObject.FindWithTag("MainCamera");
    m_button1_activated = false;
    m_button2_activated = false;
    m_button3_activated = false;
  }

  private void Update(){
    if(m_lockedplane?.subsumedBy != null){
      m_lockedplane = m_lockedplane.subsumedBy;
    }

    var screenCenter = 
          Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    var hits = new List<ARRaycastHit>();
    m_raycastmanager.Raycast(screenCenter, hits, 
            TrackableType.PlaneWithinBounds);
    m_currentplane = null;
    ARRaycastHit? hit = null;
    if(hits.Count > 0){
      // If you don't have a locked plane already...
      hit = m_lockedplane == null
              // ... use the first hit in `hits`.
      ? hits[0]
              // Otherwise use the locked plane, if it's there.
      : hits.SingleOrDefault(x => x.trackableId == 
                m_lockedplane.trackableId);
    }

    // Move the reticle and instantiate an object there if the
    // button was pressed
    if(hit.HasValue){
      m_currentplane = m_planemanager.GetPlane(hit.Value.trackableId);
      // Move reticle to the location of the hit
      transform.position = hit.Value.pose.position;
     if( m_button1_activated ){
        m_button1_activated = false;
        // Instantiate an object at the hit plane
        m_go_array[m_array_ctr] = Instantiate(m_prefab1);
        m_go_array[m_array_ctr].transform.position = hit.Value.pose.position;
        m_array_ctr += 1;
        Debug.Log("PERK object made " + m_array_ctr);
      }

     if( m_button2_activated ){
        m_button2_activated = false;
        // Instantiate an object at the hit plane
        m_go_array[m_array_ctr] = Instantiate(m_prefab2);
        m_go_array[m_array_ctr].transform.position = hit.Value.pose.position;
        m_array_ctr += 1;
        Debug.Log("PERK object made " + m_array_ctr);
      }
      if( m_button3_activated ){
        m_button3_activated = false;
        // Instantiate an object at the hit plane
        m_go_array[m_array_ctr] = Instantiate(m_prefab3);
        m_go_array[m_array_ctr].transform.position = hit.Value.pose.position;
        m_array_ctr += 1;
        Debug.Log("PERK object made " + m_array_ctr);
      }

    } // hit.HasValue
  } // update




  public void do_button_1(){
    Debug.Log("PERK button 1");
    m_button1_activated = true;
    return;
  }


  public void do_button_2(){      
    Debug.Log("PERK button 2");
    m_button2_activated = true;
    return;
  }

    public void do_button_3(){      
    Debug.Log("PERK button 3");
    m_button3_activated = true;
    return;
  }

    public void reset_button(){
        
        foreach (var go in m_go_array){
            if (go != null){
                Destroy(go);
    }
}
    m_array_ctr = 0;
    }


}
