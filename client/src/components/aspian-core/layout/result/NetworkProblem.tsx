import React, { FC } from 'react';
import { Result, Button } from 'antd';
import { withTranslation, WithTranslation } from 'react-i18next';

const NetworkProblem: FC<WithTranslation> = ({t}) => {
  return (
    <Result
      status="500"
      title={t("result-pages.network-problem.title")}
      subTitle={t("result-pages.network-problem.subtitle")}
      extra={
        <Button type="primary" href="/admin">
          {t("result-pages.network-problem.button")}
        </Button>
      }
    />
  );
};

export default withTranslation()(NetworkProblem);
