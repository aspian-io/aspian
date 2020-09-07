import React from 'react';
import { Result, Button } from 'antd';
import { history } from '../../../..';
import { useTranslation } from 'react-i18next';

const Unathorized401 = () => {
  const { t } = useTranslation('core_unathorized401Page');

  return (
    <Result
      status="403"
      title={t('title')}
      subTitle={t('subTitle')}
      extra={
        <Button type="primary" onClick={() => history.push('/login')}>
          {t('btn')}
        </Button>
      }
    />
  );
};

export default Unathorized401;
