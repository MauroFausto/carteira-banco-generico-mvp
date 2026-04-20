import "overlayscrollbars/styles/overlayscrollbars.min.css";
import { OverlayScrollbars } from "overlayscrollbars";

export const customScrollbars = () => {
	const SCROLL_ELEMS = document.querySelectorAll("[data-scrollbar]");

	SCROLL_ELEMS.forEach((elem) => {
		OverlayScrollbars(elem, {
			scrollbars: {
				autoHide: "leave",
			},
		});
	});
};
