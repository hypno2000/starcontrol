using System;

public class Tuple<T1, T2> {

	public T1 obj1;
	public T2 obj2;

	public Tuple(T1 obj1, T2 obj2) {
		this.obj1 = obj1;
		this.obj2 = obj2;
	}

	public override int GetHashCode() {
		return obj1.GetHashCode() + obj2.GetHashCode() * 100;
	}

	public override bool Equals(System.Object obj) {

		// if parameter is null return false
		if (obj == null) {
			return false;
		}

		// if parameter cannot be cast to Point return false
		Tuple<T1, T2> p = obj as Tuple<T1, T2>;
		if ((System.Object)p == null) {
			return false;
		}

		// return true if the fields match:
		return obj1.Equals(p.obj1) && obj2.Equals(p.obj2);
	}

	public bool Equals(Tuple<T1, T2> p) {

		// if parameter is null return false
		if ((object)p == null) {
			return false;
		}

		// return true if the fields match
		return obj1.Equals(p.obj1) && obj2.Equals(p.obj2);
	}

}
