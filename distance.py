def hamming_distance(s, t):
	distance = 0

	for i in range(len(s)):
		if s[i] != t[i]: distance += 1

	return distance



with open('rosalind_hamm.txt') as input:
	string_1 = input.readline().strip()
	string_2 = input.readline().strip()

print hamming_distance(string_1, string_2)