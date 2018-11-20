using Nss.Udt.Referee;

public class SuperCubeReferee : RefereeManager<SuperCube> {
	// No implementation required outside of inherited
	// and specifying the monobehaviour that will be
	// stored in the cache via generic <T>
	//
	// This script has two requirements:
	// 1] Must be added to a gameobject in the scene
	// 2] Must be added to Script Execution Order
	//    before DEFAULT TIME, specifically before
	//    the generic <T> script.
}