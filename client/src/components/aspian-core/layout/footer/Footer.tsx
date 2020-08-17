import React, { FC } from 'react';
import { Layout } from 'antd';
import { HeartFilled } from '@ant-design/icons';
import { withTranslation, WithTranslation, Trans } from 'react-i18next';
import { e2p } from '../../../../js/aspian-core/base/utilities';
import moment from 'jalali-moment';
import {
  LocaleVariableEnum,
  LanguageActionTypeEnum,
} from '../../../../app/stores/actions/aspian-core/locale/types';

const { Footer } = Layout;
const AspianFooter: FC<WithTranslation> = ({ t }) => {
  const langFromLocalStorage = localStorage.getItem(
    LocaleVariableEnum.ASPIAN_CMS_LANG
  );
  const persianYear = e2p(moment().locale('fa').format('YYYY').toString());
  return (
    <Footer style={{ textAlign: 'center', fontSize: '.65rem' }}>
      <div>
        <Trans>{t('footer.made-with')}</Trans>
        <HeartFilled style={{ color: 'red' }} />
        <Trans>{t('footer.by')}</Trans>
        <a href="#!">
          <Trans>{t('footer.my-name')}</Trans>
        </a>
      </div>
      <div>
        <Trans>{t('appName')}</Trans> - <Trans>{t('footer.copyright')}</Trans>{' '}
        &copy;{' '}
        {langFromLocalStorage === LanguageActionTypeEnum.fa
          ? persianYear
          : new Date().getFullYear()}
      </div>
    </Footer>
  );
};

export default withTranslation()(AspianFooter);
