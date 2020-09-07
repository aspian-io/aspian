import i18n from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
//import XHR from 'i18next-xhr-backend';
import { initReactI18next } from "react-i18next";

// aspian-core ---> US-en translation files
import core__common_US_en from './en/aspian-core/common/common_US-en.json';
import core__dashboard_US_en from './en/aspian-core/dashboard/dashboard_US-en.json';
import core__breadcrumb_US_en from './en/aspian-core/layout/breadcrumb/breadcrumb_US-en.json';
import core__footer_US_en from './en/aspian-core/layout/footer/footer_US-en.json';
import core__header_US_en from './en/aspian-core/layout/header/header_US-en.json';
import core__badRequest_US_en from './en/aspian-core/layout/result/badRequest_US-en.json';
import core__networkProblem_US_en from './en/aspian-core/layout/result/networkProblem_US-en.json';
import core__notFound_US_en from './en/aspian-core/layout/result/notFound_US-en.json';
import core__resultPage_US_en from './en/aspian-core/layout/result/resultPage_US-en.json';
import core__serverError_US_en from './en/aspian-core/layout/result/serverError_US-en.json';
import core__unathorized401_US_en from './en/aspian-core/layout/result/unathorized401_US-en.json';
import core__unathorized403_US_en from './en/aspian-core/layout/result/unathorized403_US-en.json';
import core__error_US_en from './en/aspian-core/layout/result/error_US-en.json';
import core__sider_US_en from './en/aspian-core/layout/sider/sider_US-en.json';
import core__menu_US_en from './en/aspian-core/layout/sider/menu/menu_US-en.json';
import core__postList_US_en from './en/aspian-core/post/postList/postList_US-en.json';
import core__postDetails_US_en from './en/aspian-core/post/postDetails/postDetails_US-en.json';
import core__login_US_en from './en/aspian-core/user/login_US-en.json';
import core__register_US_en from './en/aspian-core/user/register_US-en.json';

// aspian-core ---> IR-fa translation files
import core__common_IR_fa from './fa/aspian-core/common/common_IR-fa.json';
import core__dashboard_IR_fa from './fa/aspian-core/dashboard/dashboard_IR-fa.json';
import core__breadcrumb_IR_fa from './fa/aspian-core/layout/breadcrumb/breadcrumb_IR-fa.json';
import core__footer_IR_fa from './fa/aspian-core/layout/footer/footer_IR-fa.json';
import core__header_IR_fa from './fa/aspian-core/layout/header/header_IR-fa.json';
import core__badRequest_IR_fa from './fa/aspian-core/layout/result/badRequest_IR-fa.json';
import core__networkProblem_IR_fa from './fa/aspian-core/layout/result/networkProblem_IR-fa.json';
import core__notFound_IR_fa from './fa/aspian-core/layout/result/notFound_IR-fa.json';
import core__resultPage_IR_fa from './fa/aspian-core/layout/result/resultPage_IR-fa.json';
import core__serverError_IR_fa from './fa/aspian-core/layout/result/serverError_IR-fa.json';
import core__unathorized401_IR_fa from './fa/aspian-core/layout/result/unathorized401_IR-fa.json';
import core__unathorized403_IR_fa from './fa/aspian-core/layout/result/unathorized403_IR-fa.json';
import core__error_IR_fa from './fa/aspian-core/layout/result/error_IR-fa.json';
import core__sider_IR_fa from './fa/aspian-core/layout/sider/sider_IR-fa.json';
import core__menu_IR_fa from './fa/aspian-core/layout/sider/menu/menu_IR-fa.json';
import core__postList_IR_fa from './fa/aspian-core/post/postList/postList_IR-fa.json';
import core__postDetails_IR_fa from './fa/aspian-core/post/postDetails/postDetails_IR-fa.json';
import core__login_IR_fa from './fa/aspian-core/user/login_IR-fa.json';
import core__register_IR_fa from './fa/aspian-core/user/register_IR-fa.json';

i18n
  .use(initReactI18next)
  // .use(XHR)
  .use(LanguageDetector)
  .init({
    debug: false,
    lng: 'fa',
    fallbackLng: 'en', // use en if detected lng is not available

    //keySeparator: false, // we do not use keys in form messages.welcome

    interpolation: {
      escapeValue: false, // react already safes from xss
    },

    resources: {
      en: {
        core_common: core__common_US_en,
        core_dashboard: core__dashboard_US_en,
        core_breadcrumb: core__breadcrumb_US_en,
        core_footer: core__footer_US_en,
        core_header: core__header_US_en,
        core_badRequestPage: core__badRequest_US_en,
        core_networkProblemPage: core__networkProblem_US_en,
        core_notFoundPage: core__notFound_US_en,
        core_resultPage: core__resultPage_US_en,
        core_serverErrorPage: core__serverError_US_en,
        core_unathorized401Page: core__unathorized401_US_en,
        core_unathorized403Page: core__unathorized403_US_en,
        core_error: core__error_US_en,
        core_sider: core__sider_US_en,
        core_menu: core__menu_US_en,
        core_postList: core__postList_US_en,
        core_postDetails: core__postDetails_US_en,
        core_login: core__login_US_en,
        core_register: core__register_US_en,
      },
      fa: {
        core_common: core__common_IR_fa,
        core_dashboard: core__dashboard_IR_fa,
        core_breadcrumb: core__breadcrumb_IR_fa,
        core_footer: core__footer_IR_fa,
        core_header: core__header_IR_fa,
        core_badRequestPage: core__badRequest_IR_fa,
        core_networkProblemPage: core__networkProblem_IR_fa,
        core_notFoundPage: core__notFound_IR_fa,
        core_resultPage: core__resultPage_IR_fa,
        core_serverErrorPage: core__serverError_IR_fa,
        core_unathorized401Page: core__unathorized401_IR_fa,
        core_unathorized403Page: core__unathorized403_IR_fa,
        core_error: core__error_IR_fa,
        core_sider: core__sider_IR_fa,
        core_menu: core__menu_IR_fa,
        core_postList: core__postList_IR_fa,
        core_postDetails: core__postDetails_IR_fa,
        core_login: core__login_IR_fa,
        core_register: core__register_IR_fa,
      },
    },
    // have a common namespace used around the full app
    // ns: ['postDetails', 'translations'],
    // defaultNS: 'translations',
  });

export default i18n;
