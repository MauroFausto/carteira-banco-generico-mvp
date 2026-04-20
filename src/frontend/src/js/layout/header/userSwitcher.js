import { getAuthContext, setAuthContext } from "../../api/client";

export const userSwitcher = () => {
	const select = document.getElementById("cb-user-switcher");
	if (!select) {
		return;
	}

	const { userId, userRole } = getAuthContext();
	const currentValue = `${userId}|${userRole}`;
	const hasCurrentOption = Array.from(select.options).some((option) => option.value === currentValue);
	if (hasCurrentOption) {
		select.value = currentValue;
	}

	select.addEventListener("change", () => {
		const [nextUserId, nextRole] = select.value.split("|");
		setAuthContext({ userId: nextUserId, userRole: nextRole });
		window.location.reload();
	});
};

