namespace System.Collections.Generic
{
	/// <summary>
	/// A Bi-Directional Dictionary where keys are also values, and values are also keys.
	/// *** NO DUPLICATE KEYS OR VALUES ***.
	/// </summary>
	/// <typeparam name="TFlip"></typeparam>
	/// <typeparam name="TFlop"></typeparam>
	public class FlipFlopDictionary<TFlip, TFlop> : IDictionary<TFlip, TFlop>
	{
		private readonly IDictionary<TFlip, TFlop> Flip = new Dictionary<TFlip, TFlop>();
		private readonly IDictionary<TFlop, TFlip> Flop = new Dictionary<TFlop, TFlip>();

		public TFlop this[TFlip flip]
		{
			get => Flip[flip];
			set
			{
				Flip[flip] = value;
				Flop[value] = flip;
			}
		}
		public TFlip this[TFlop flop]
		{
			get => Flop[flop];
			set
			{
				Flop[flop] = value;
				Flip[value] = flop;
			}
		}

		public ICollection<TFlip> Keys => Flip.Keys;

		public ICollection<TFlop> Values => Flop.Keys;

		public int Count => Flip.Count;

		public bool IsReadOnly => Flip.IsReadOnly;

		public void Add(TFlip flip, TFlop flop)
		{
			Flip.Add(flip, flop);
			Flop.Add(flop, flip);
		}

		public void Add(KeyValuePair<TFlip, TFlop> flipFlop)
		{
			var flopFlip = new KeyValuePair<TFlop, TFlip>(flipFlop.Value, flipFlop.Key);
			Flip.Add(flipFlop);
			Flop.Add(flopFlip);
		}

		public void Clear()
		{
			Flip.Clear();
			Flop.Clear();
		}

		public bool Contains(KeyValuePair<TFlip, TFlop> flipFlop) => Flip.Contains(flipFlop);
		public bool Contains(KeyValuePair<TFlop, TFlip> flopFlip) => Flop.Contains(flopFlip);

		public bool ContainsKey(TFlip flip) => Flip.ContainsKey(flip);
		public bool ContainsKey(TFlop flop) => Flop.ContainsKey(flop);

		public void CopyTo(KeyValuePair<TFlip, TFlop>[] array, int arrayIndex) => Flip.CopyTo(array, arrayIndex);
		public void CopyTo(KeyValuePair<TFlop, TFlip>[] array, int arrayIndex) => Flop.CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<TFlip, TFlop>> GetEnumerator() => Flip.GetEnumerator();

		public bool Remove(TFlip flipKey)
		{
			var flopKey = Flip[flipKey];
			return Flip.Remove(flipKey) && Flop.Remove(flopKey);
		}
		public bool Remove(TFlop flopKey)
		{
			var flipKey = Flop[flopKey];
			return Flop.Remove(flopKey) && Flip.Remove(flipKey);
		}

		public bool Remove(KeyValuePair<TFlip, TFlop> flipFlop)
		{
			var flopFlip = new KeyValuePair<TFlop, TFlip>(flipFlop.Value, flipFlop.Key);
			return Flip.Remove(flipFlop) && Flop.Remove(flopFlip);
		}

		public bool TryGetValue(TFlip flip, out TFlop flop) => Flip.TryGetValue(flip, out flop);
		public bool TryGetValue(TFlop flop, out TFlip flip) => Flop.TryGetValue(flop, out flip);

		IEnumerator IEnumerable.GetEnumerator() => Flip.GetEnumerator();
	}
}