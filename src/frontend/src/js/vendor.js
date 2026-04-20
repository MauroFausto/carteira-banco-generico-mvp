import { vanillajsDatepicker } from "./vendors/vanillaJsDatepicker";
import { quillEditor } from "./vendors/quill";
import { bootstrapComponents } from "./vendors/bootstrap";
import { slider } from "./vendors/noUiSlider";
import { colorPicker } from "./vendors/vanillaColorful";
import { tableBasic } from "./vendors/simple-datatables/basic";
import { tableExport } from "./vendors/simple-datatables/export";
import { tableFilter } from "./vendors/simple-datatables/filter";
import { tableCheckbox } from "./vendors/simple-datatables/checkbox";
import { tableScroll } from "./vendors/simple-datatables/scroll";
import { customScrollbars } from "./vendors/overlay-scrollbars";

// Overlay Scrollbars
customScrollbars();

// Phosphor icons
import "@phosphor-icons/web/src/light/style.css";
import "@phosphor-icons/web/src/regular/style.css";

// Bootstrap + compomnents
import * as bootstrap from "bootstrap";
bootstrapComponents();

// Vanilla JS Datepicker
vanillajsDatepicker();

// Quill
quillEditor();

// noUiSlider
slider();

// Vanilla Colorful
colorPicker();

// Simple Datatables
tableBasic();
tableExport();
tableFilter();
tableCheckbox();
tableScroll();
