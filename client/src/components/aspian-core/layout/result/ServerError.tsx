import React, { FC } from 'react';
import { Result, Button } from 'antd';
import { withTranslation, WithTranslation } from 'react-i18next';

const ServerError: FC<WithTranslation> = ({t}) => {
  return (
    <Result
      status="500"
      title={t("result-pages.500.title")}
      subTitle={t("result-pages.500.subtitle")}
      extra={
        <Button type="primary" href="/admin">
          {t("result-pages.500.button")}
        </Button>
      }
    />
  );
};

export default withTranslation()(ServerError);
