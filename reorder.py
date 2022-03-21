def num_rabbits(months, litter):
	if months == 0: return 0
	if months == 1: return 1
	answer = num_rabbits(months-1, litter) + litter*num_rabbits(months-2, litter)
	return answer

print num_rabbits(35, 3)