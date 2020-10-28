import React from 'react';
import { Layout } from 'antd';
import { HeartFilled } from '@ant-design/icons';
import { useTranslation } from 'react-i18next';
import { e2p } from '../../../../js-ts/aspian-core/base/NumberConverter';
import moment from 'jalali-moment';
import {
  LocaleVariableEnum,
  LanguageActionTypeEnum,
} from '../../../../app/stores/aspian-core/locale/types';

const { Footer } = Layout;
const AspianFooter = () => {
  const { t } = useTranslation(['core_footer', 'core_common']);
  const langFromLocalStorage = localStorage.getItem(
    LocaleVariableEnum.ASPIAN_CMS_LANG
  );
  const persianYear = e2p(moment().locale('fa').format('YYYY').toString());
  return (
    <Footer style={{ textAlign: 'center', fontSize: '.65rem' }}>
      <div>
        {t('made-with')}
        <HeartFilled style={{ color: 'red' }} />
        {t('by')}
        <a href="#!">
          {t('my-name')}
        </a>
      </div>
      <div>
        {t('core_common:appName')} - {t('copyright')}{' '}
        &copy;{' '}
        {langFromLocalStorage === LanguageActionTypeEnum.fa
          ? persianYear
          : new Date().getFullYear()}
      </div>
    </Footer>
  );
};

export default AspianFooter;
